package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.mapper.FlightMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.DirVO;
import com.servetech.dynarap.vo.FlightVO;
import lombok.Data;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Array;
import java.util.*;

@Service("dirService")
public class DirService {

    @Autowired
    private DirMapper dirMapper;

    // 사용자 디렉토리 전체
    public JsonObject getDirList(CryptoField.NAuth uid) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uid", uid);
            List<DirVO> userDirs = dirMapper.selectUserDirList(params);
            if (userDirs == null) userDirs = new ArrayList<>();

            Map<String, DirVO> dirMap = new LinkedHashMap<>();
            for (DirVO userDir : userDirs)
                dirMap.put(userDir.getSeq().valueOf(), userDir);

            List<DirTreeItem> presentation = new ArrayList<>();
            List<DirVO> rootDirs = getSubDirList(uid, CryptoField.LZERO);
            if (rootDirs == null) rootDirs = new ArrayList<>();
            for (DirVO rootDir : rootDirs) {
                DirTreeItem dti = new DirTreeItem();
                dti.setDirKey(rootDir.getSeq().valueOf());
                dti.setSubTree(new ArrayList<>());
                presentation.add(dti);
                __genPresentation(uid, getSubDirList(uid, rootDir.getSeq()), dti.getSubTree());
            }

            JsonObject dirInfo = new JsonObject();
            dirInfo.add("pools", ServerConstants.GSON.toJsonTree(dirMap));
            dirInfo.add("presentation", ServerConstants.GSON.toJsonTree(presentation));

            return dirInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    private void __genPresentation(CryptoField.NAuth uid, List<DirVO> rootDirs, List<DirTreeItem> parentList) throws HandledServiceException {
        if (rootDirs == null || rootDirs.size() == 0) return;
        for (DirVO dir : rootDirs) {
            DirTreeItem dti = new DirTreeItem();
            dti.setDirKey(dir.getSeq().valueOf());
            dti.setSubTree(new ArrayList<>());
            parentList.add(dti);
            __genPresentation(uid, getSubDirList(uid, dir.getSeq()), dti.getSubTree());
        }
    }

    @Data
    private static class DirTreeItem {
        private String dirKey;
        private List<DirTreeItem> subTree;
    }

    // 측정 상위 폴더를 갖는 하위 폴더 전체.
    public List<DirVO> getSubDirList(CryptoField.NAuth uid, CryptoField parentDirSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uid", uid);
            params.put("parentDirSeq", parentDirSeq);
            return dirMapper.selectUserDirListByParent(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public DirVO getDir(CryptoField dirSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", dirSeq);
            return dirMapper.selectUserDir(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public DirVO insertDir(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DirVO dir = ServerConstants.GSON.fromJson(payload, DirVO.class);
            if (dir == null)
                throw new HandledServiceException(411, "요청 내용이 폴더/파일 형식에 맞지 않습니다.");

            dir.setUid(uid);
            dir.setCreatedAt(LongDate.now());
            dirMapper.insertDir(dir);

            // 상위 디렉토리 세팅.
            __fillParentInfo(dir);

            // TODO : 관련 정보 있으면 채워야 함.

            return dir;

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    private void __fillParentInfo(DirVO dir) throws  HandledServiceException {
        if (dir.getParentDirSeq() != null && !dir.getParentDirSeq().isEmpty()) {
            dir.setParentDirInfo(getDir(dir.getParentDirSeq()));
            DirVO parentInfo = dir.getParentDirInfo();
            while (parentInfo != null) {
                if (parentInfo.getParentDirSeq() == null || parentInfo.getParentDirSeq().isEmpty()) {
                    parentInfo.setParentDirInfo(null);
                    break;
                }
                parentInfo.setParentDirInfo(getDir(parentInfo.getParentDirSeq()));
                parentInfo = parentInfo.getParentDirInfo();
            }
        }
        else {
            dir.setParentDirInfo(null);
        }
    }

    @Transactional
    public DirVO updateDir(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DirVO dir = ServerConstants.GSON.fromJson(payload, DirVO.class);
            if (dir == null || dir.getSeq() == null || dir.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            dirMapper.updateDir(dir);

            __fillParentInfo(dir);

            return dir;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDir(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dirSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                dirSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (dirSeq == null || dirSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            // 재귀 삭제.
            DirVO dir = getDir(dirSeq);
            List<DirVO> removeList = new ArrayList<>();
            removeList.add(dir);

            int indicator = removeList.size() - 1;
            while (indicator < removeList.size()) {
                List<DirVO> children = getSubDirList(uid, removeList.get(indicator).getSeq());
                for (DirVO d : children)
                    removeList.add(d);
                indicator++;
            }

            for (int i = removeList.size() - 1; i >= 0; i--) {
                Map<String, Object> params = new HashMap<>();
                params.put("seq", removeList.get(i).getSeq());
                dirMapper.deleteDir(params);
            }

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }
}
