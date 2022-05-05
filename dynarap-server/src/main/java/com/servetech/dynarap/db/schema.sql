use `dynarap`;

delimiter $$


-- oauth 정보는 nauth에 위임

-- 회원 테이블
drop table if exists `dynarap_user` cascade $$

create table `dynarap_user`
(
    `uid` bigint not null,                          -- 일련번호 base (use nauth)
    `userType` tinyint default 9,                   -- 사용자 타입
    `username` varchar(128) not null,               -- 사용자 ID
    `password` varchar(256) not null,               -- 비밀번호 HmacSHA256
    `provider` varchar(32) not null,                -- 정보제공자
    `joinedAt` bigint default 0,                    -- 가입일자
    `leftAt` bigint default 0,                      -- 탈퇴일자
    `email` varchar(256) null,                      -- 이메일 주소
    `accountLocked` tinyint default 0,              -- 계정 잠금(휴먼 등)
    `accountName` varchar(64) not null,             -- 사용자 이름
    `profileUrl` varchar(512) default '',           -- 프로필 주소
    `phoneNumber` varchar(32) null,                 -- 핸드폰 번호
    `privacyTermsReadAt` bigint default 0,          -- 개인정보취급방침동의
    `serviceTermsReadAt` bigint default 0,          -- 서비스약관동의
    `pushToken` varchar(512) null,                  -- 푸시토큰
    `usePush` tinyint default 0,                    -- 푸시 사용
    `tempPassword` varchar(256),                    -- 임시 비번
    `tempPasswordExpire` bigint default 0,          -- 임시 비번 만료
    constraint pk_dynarap_user primary key (`uid`)
) $$

insert into dynarap_user(uid, userType, username, password, provider, joinedAt, leftAt, email, accountLocked, accountName, phoneNumber, privacyTermsReadAt, serviceTermsReadAt, pushToken, usePush)
values (10017546, 0, 'admin@dynarap', 'd19ed59ffded1fc1c664361fd7f89a9ce1ade657d5eba9e21470cac17f0706c3', 'neoulsoft',
        unix_timestamp() * 1000, 0, 'admin@dynarap', 0, '서비스관리자', '01046180526', unix_timestamp() * 1000, unix_timestamp() * 1000, '', 1) $$

-- 비행기체 관리
drop table if exists `dynarap_flight` cascade $$

create table `dynarap_flight`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `flightName` varchar(64) not null               comment '비행기체 이름',
    `flightType` varchar(64)                        comment '비행기체 종류',
    `createdAt` bigint default 0                    comment '제조일',
    `registeredAt` bigint default 0                 comment '등록일',
    `registerUid` bigint default 0                  comment '등록자',
    `lastFlightAt` bigint default 0                 comment '마지막 비행일시',
    constraint pk_dynarap_flight primary key (`seq`)
) $$


-- 디렉토리 관리
drop table if exists `dynarap_dir` cascade $$

create table `dynarap_dir`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `parentDirSeq` bigint default 0                 comment '상위 디렉토리 일련번호',
    `uid` bigint not null                           comment '소유자 일련번호',
    `dirName` varchar(64) not null                  comment '디렉토리 이름',
    `dirType` varchar(32)                           comment '디렉토리 타입 (folder, file, param, preset, part, shortblock)',
    `dirIcon` varchar(256)                          comment '디렉토리 아이콘',
    `createdAt` bigint default 0                    comment '생성일자',
    `refSeq` bigint default 0                       comment '참조키',
    `refSubSeq` bigint default 0                    comment '참조키(보조)',
    constraint pk_dynarap_dir primary key (`seq`)
) $$


-- 파라미터 관련 먼저 구현
drop table if exists `dynarap_param` cascade $$

create table `dynarap_param`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `paramPack` bigint default 0                    comment '이력일련번호',
    `paramGroupSeq` bigint default 0                comment '파라미터 그룹',
    `paramKey` varchar(64) not null                 comment '파라미터 고유키',
    `paramSpec` varchar(32)                         comment '파라미터 특성',
    `adamsKey` varchar(64)                          comment 'ADAMS 고유키',
    `zaeroKey` varchar(64)                          comment 'ZAERO 고유키',
    `grtKey` varchar(64)                            comment 'GRP 고유키',
    `fltpKey` varchar(64)                           comment 'FLTP 고유키',
    `fltsKey` varchar(64)                           comment 'FLTS 고유키',
    `partInfo` varchar(32)                          comment '파트정보',
    `partInfoSub` varchar(32)                       comment '파트 부가정보',
    `lrpX` double default 0.0                       comment 'LRP X',
    `lrpY` double default 0.0                       comment 'LRP Y',
    `lrpZ` double default 0.0                       comment 'LRP Z',
    `paramUnit` varchar(32)                         comment '파라미터 단위',
    `domainMin` double default 0.0                  comment '파라미터 범위 최저값',
    `domainMax` double default 0.0                  comment '파라미터 범위 최대값',
    `specified` double default 0.0                  comment '파라미터 지정 값',
    `paramVal` varchar(1024)                        comment '파라미터 지정 값 (숫자외)',
    `registerUid` bigint default 0                  comment '등록자',
    `appliedAt` bigint default 0                    comment '정보 적용일자',
    `appliedEndAt` bigint default 0                 comment '정보 만료일자',
    constraint pk_dynarap_param primary key (`seq`)
) $$


-- 파라미터 그룹 정보 (마하, Q, AOS 등등)
drop table if exists `dynarap_param_group` cascade $$

create table `dynarap_param_group`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `groupName` varchar(32) not null                comment '파라미터 그룹 이름',
    `groupType` varchar(32)                         comment '파라미터 그룹 타입',
    `registerUid` bigint default 0                  comment '등록자',
    `createdAt` bigint default 0                    comment '생성일자',
    constraint pk_dynarap_param_group primary key (`seq`)
) $$


-- 프리셋 정보
drop table if exists `dynarap_preset` cascade $$

create table `dynarap_preset`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `presetPack` bigint default 0                   comment '프리셋 관리번호',
    `presetName` varchar(64) not null               comment '프리셋 이름',
    `presetPackFrom` bigint default 0               comment '프리셋 구성 복사 원본',
    `createdAt` bigint default 0                    comment '프리셋 생상일자',
    `registerUid` bigint default 0                  comment '등록자',
    `appliedAt` bigint default 0                    comment '프리셋 적용일자',
    `appliedEndAt` bigint default 0                 comment '프리셋 만료일자',
    constraint pk_dynarap_preset primary key (`seq`)
) $$


-- 프리셋 구성 파라미터 정보
drop table if exists `dynarap_preset_param` cascade $$

create table `dynarap_preset_param`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `presetPack` bigint default 0                   comment '프리셋 관리번호',
    `presetSeq` bigint default 0                    comment '프리셋 일련번호 (구성당시)',
    `paramPack` bigint default 0                    comment '파라미터 관리번호',
    `paramSeq` bigint default 0                     comment '파라미터 일련번호 (구성당시)',
    constraint pk_dynarap_preset_param primary key (`seq`)
) $$


-- 관련 데이터 업로드 -> 이후 파일 정보로 raw 데이터 구성 후 새로 잘라낼 수 있음
drop table if exists `dynarap_raw_upload` cascade $$

create table `dynarap_raw_upload`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `uploadId` varchar(256) not null                comment '데이터 ID',
    `uploadName` varchar(128) not null              comment '데이터 이름',
    `storePath` varchar(512)                        comment '저장 경로 (서버상 파일 위치)',
    `fileSize` bigint default 0                     comment '파일용량',
    `flightSeq` bigint default 0                    comment '관련 기체, 없으면 0',
    `presetPack` bigint default 0                   comment '프리셋 관리번호',
    `presetSeq` bigint default 0                    comment '프리셋 일련번호',
    `uploadedAt` bigint default 0                   comment '업로드 일시',
    `flightAt` bigint default 0                     comment '비행시간 (참고용)',
    `registerUid` bigint default 0                  comment '등록자',
    constraint pk_dynarap_raw_upload primary key (`seq`)
) $$


-- dll 데이터 수기 입력
drop table if exists `dynarap_dll` cascade $$

create table `dynarap_dll`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `dataSetCode` varchar(32) not null              comment '데이터 코드',
    `dataSetName` varchar(64) not null              comment '데이터 이름',
    `dataVersion` varchar(16)                       comment '데이터 버전',
    `registerUid` bigint default 0                  comment '등록자',
    `createdAt` bigint default 0                    comment '생성일자',
    constraint pk_dynarap_dll primary key (`seq`)
) $$

-- dll parameter set
drop table if exists `dynarap_dll_param` cascade $$

create table `dynarap_dll_param`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `dllSeq` bigint not null                        comment '데이터 셋 일련번호',
    `paramName` varchar(64) not null                comment '파라미터 이름',
    `paramType` varchar(32)                         comment '파라미터 타입',
    `paramNo` smallint default 0                    comment '파라미터 순서',
    `registerUid` bigint default 0                  comment '등록자',
    constraint pk_dynarap_dll_param primary key (`seq`)
) $$

-- dll parameter table
drop table if exists `dynarap_dll_raw` cascade $$

create table `dynarap_dll_raw`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `dllSeq` bigint not null                        comment '데이터 셋 일련번호',
    `paramSeq` bigint not null                      comment '파라미터 일련번호',
    `rowNo` int default 0                           comment '데이터 row',
    `paramVal` double default 0.0                   comment '파라미터 값 (숫자)',
    `paramValStr` varchar(128) default ''           comment '파라미터 값 (문자)',
    constraint pk_dynarap_dll_raw primary key (`seq`)
) $$

create index idx_dll_raw on `dynarap_dll_raw` (`dllSeq`, `paramSeq`, `rowNo`) $$


-- import module (dll 은 수동 입력, zaero, adams, 시험비행 데이터 있음)
-- raw 데이터는 입력후 삭제
drop table if exists `dynarap_raw` cascade $$

create table `dynarap_raw`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `uploadSeq` bigint default 0                    comment '업로드 일련번호',
    `presetPack` bigint default 0                   comment '프리셋 관리번호',
    `presetSeq` bigint default 0                    comment '프리셋 일련번호',
    `presetParamSeq` bigint default 0               comment '프리셋 구성 파라미터 일련번호',
    `rowNo` int default 0                           comment '데이터 row',
    `julianTimeAt` varchar(32)                      comment '절대 시간 값',
    `paramVal` double default 0.0                   comment '파라미터 값 (숫자)',
    `paramValStr` varchar(128) default ''           comment '파라미터 값 (문자)',
    constraint pk_dynarap_raw primary key (`seq`)
) $$

create index idx_raw on `dynarap_raw` (`presetPack`, `presetSeq`, `presetParamSeq`, `rowNo`) $$

-- 유의미한 분할 데이터를 저장하고 해당 저장 데이터에 대해서 기본 단위로 사용함.
drop table if exists `dynarap_part` cascade $$

create table `dynarap_part`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `partName` varchar(128) not null                comment '부분 이름',
    `presetPack` bigint default 0                   comment '프리셋 관리번호',
    `presetSeq` bigint default 0                    comment '프리셋 일련번호',
    `julianStartAt` varchar(32) not null            comment '절대시간값(시작)',
    `julianEndAt` varchar(32) not null              comment '절대시간값(종료)',
    `offsetStartAt` double default 0.0              comment '상대시간값(시작)',
    `offsetEndAt` double default 0.0                comment '상대시간값(종료)',
    `registerUid` bigint default 0                  comment '등록자',
    constraint pk_dynarap_part primary key (`seq`)
) $$

create index idx_part on `dynarap_part` (`presetPack`, `presetSeq`, `seq`) $$

--  분할 구간 raw 데이터
drop table if exists `dynarap_part_raw` cascade $$

create table `dynarap_part_raw`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `partSeq` bigint not null                       comment '부분 일련번호',
    `presetParamSeq` bigint default 0               comment '프리셋 파라미터 일련번호',
    `rowNo` int default 0                           comment '데이터 row',
    `paramVal` double default 0.0                   comment '파라미터 값 (숫자)',
    `paramValStr` varchar(128) default ''           comment '파라미터 값 (문자)',
    `julianTimeAt` varchar(32) not null             comment '절대시간값',
    `offsetTimeAt` double default 0.0               comment '상대시간값',
    `lpf` double default 0.0                        comment 'LPF',
    `hpf` double default 0.0                        comment 'HPF',
    constraint pk_dynarap_part_raw primary key (`seq`)
) $$

create index idx_part_raw on `dynarap_part_raw` (`partSeq`, `presetParamSeq`, `rowNo`) $$


-- 숏블록 기본
drop table if exists `dynarap_sblock_meta` cascade $$

create table `dynarap_sblock_meta`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `partSeq` bigint not null                       comment '부분 일련번호',
    `overlap` float default 0.0                     comment '겹침구간 (%)',
    `sliceTime` float default 0.0                   comment '분할시간',
    `registerUid` bigint default 0                  comment '등록자',
    `createdAt` bigint default 0                    comment '생성일시',
    constraint pk_dynarap_sblock_meta primary key (`seq`)
) $$


-- 숏블록 데이터 분할 구간
drop table if exists `dynarap_sblock` cascade $$

create table `dynarap_sblock`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `blockMetaSeq` bigint default 0                 comment '메타 일련번호',
    `partSeq` bigint default 0                      comment '부분 일련번호',
    `julianStartAt` varchar(32) not null            comment '절대시간값(시작)',
    `julianEndAt` varchar(32) not null              comment '절대시간값(종료)',
    `offsetStartAt` double default 0.0              comment '상대시간값(시작)',
    `offsetEndAt` double default 0.0                comment '상새시간값(종료)',
    `blockNo` int default 0                         comment '블록 번호',
    `registerUid` bigint default 0                  comment '등록자',
    constraint pk_dynarap_sblock primary key (`seq`)
) $$

create index idx_sblock on `dynarap_sblock` (`partSeq`, `blockMetaSeq`, `seq`) $$

-- 숏블록 데이터
drop table if exists `dynarap_sblock_raw` cascade $$

create table `dynarap_sblock_raw`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `blockSeq` bigint not null                      comment '숏블록 일련번호',
    `partSeq` bigint not null                       comment '부분 일련번호',
    `presetParamSeq` bigint default 0               comment '프리셋 파라미터 일련번호',
    `rowNo` int default 0                           comment '데이터 row',
    `paramVal` double default 0.0                   comment '파라미터 값 (숫자)',
    `paramValStr` varchar(128) default ''           comment '파라미터 값 (문자)',
    `julianTimeAt` varchar(32) not null             comment '절대시간값',
    `offsetTimeAt` double default 0.0               comment '상대시간값',
    `lpf` double default 0.0                        comment 'LPF',
    `hpf` double default 0.0                        comment 'HPF',
    constraint pk_dynarap_sblock_raw primary key (`seq`)
) $$

create index idx_sblock_raw on `dynarap_sblock_raw` (`partSeq`, `blockSeq`, `presetParamSeq`, `rowNo`) $$

-- 숏블록 파라미터 처리
drop table if exists `dynarap_sblock_param` cascade $$

create table `dynarap_sblock_param`
(
    `seq` bigint auto_increment not null            comment '일련번호',
    `blockSeq` bigint not null                      comment '숏블록 일련번호',
    `paramNo` smallint default 0                    comment '설정 파라미터 순서',
    `paramPack` bigint default 0                    comment '설정 파라미터 관리 일련번호',
    `paramSeq` bigint default 0                     comment '설정 파라미터 일련번호',
    constraint pk_dynarap_sblock_param primary key (`seq`)
) $$



delimiter ;

