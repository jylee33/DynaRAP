package com.servetech.dynarap.db.service;

import com.servetech.dynarap.db.mapper.UserMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.UserVO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("userService")
public class UserService {

    @Autowired
    private UserMapper userMapper;

    public UserVO getUser(String username) throws HandledServiceException {
        try {
            if (!username.contains("@"))
                throw new HandledServiceException(400, "Invalid account format. [" + username + "]");

            String accountId = username.substring(0, username.lastIndexOf("@"));
            String clientId = username.substring(username.lastIndexOf("@") + 1);

            Map<String, Object> params = new HashMap<String, Object>();
            params.put("username", accountId);
            UserVO user = userMapper.selectUserByUsername(params);
            return user;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public UserVO getUserByProvider(String username, String provider) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<String, Object>();
            params.put("username", username);
            params.put("provider", provider);
            UserVO user = userMapper.selectUserByProvider(params);
            return user;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public UserVO getUserBySeq(CryptoField.NAuth uid) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<String, Object>();
            params.put("uid", uid);
            UserVO user = userMapper.selectUserBySeq(params);
            return user;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public UserVO getUserByParams(Map<String, Object> params) throws HandledServiceException {
        try {
            UserVO user = userMapper.selectUserByParams(params);
            return user;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public boolean isDuplicateEmail(String email) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("username", email);
            UserVO user = userMapper.selectUserByUsername(params);
            return user != null;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertUser(UserVO user) throws HandledServiceException {
        try {
            userMapper.insertUser(user);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateUser(UserVO user) throws HandledServiceException {
        try {
            userMapper.updateUser(user);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertUserDevice(UserVO.Device userDevice) throws HandledServiceException {
        try {
            userMapper.insertUserDevice(userDevice);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateUserDevice(UserVO.Device userDevice) throws HandledServiceException {
        try {
            userMapper.updateUserDevice(userDevice);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteUserDevice(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            userMapper.deleteUserDevice(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteUserDevices(CryptoField.NAuth uid) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uid", uid);
            userMapper.deleteUserDevices(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<UserVO.Device> getUserDevices(CryptoField.NAuth uid) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uid", uid);
            return userMapper.selectUserDevices(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public UserVO.Device getUserDeviceBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            return userMapper.selectUserDeviceBySeq(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public UserVO.Device getUserDeviceByAdid(String adid) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("adid", adid);
            return userMapper.selectUserDeviceByAdid(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }


    public List<UserVO> findAccounts(String accountName, String phoneNumber) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("accountName", accountName);
            params.put("phoneNumber", phoneNumber);
            return userMapper.selectFindAccounts(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public boolean isExistAccount(CryptoField.NAuth uid, String email) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uid", uid);
            params.put("email", email);
            return userMapper.selectUserByIdEmail(params) > 0;
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }


    public UserVO getAccountByNamePhone(Map<String, Object> params) throws HandledServiceException {
        try {
            return userMapper.selectAccountByNamePhone(params);
        } catch (Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
