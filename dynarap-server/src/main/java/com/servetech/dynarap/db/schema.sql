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



-- 시나리오
drop table if exists `dynarap_scenario` cascade $$

create table `dynarap_scenario`
(
    `seq` bigint auto_increment not null,       -- 시나리오 일련번호
    `scenarioName` varchar(128) not null,       -- 시나리오 이름
    `scenarioType` varchar(32) not null,        -- 시나리오 타입 (import, parameter, basic, buffet, etc)
    `createdAt` bigint default 0,               -- 시나리오 생성일자
    `enabled` tinyint default 1,                -- 시나리오 활성화 여부
    `scenarioGrade` tinyint default 99,         -- 시나리오 등급, 관리 목적, 낮을 수록 상위권한
    `ownerUid` bigint default 0,                -- 시나리오 소유자
    `flightSeq` bigint default 0,               -- 비행 기체 선택
    `flightDate` bigint default 0,              -- 비행날짜
    constraint pk_dynarap_scenraio primary key (`seq`)
) $$

-- 시나리오 열람 가능자
drop table if exists `dynarap_scenario_allowed` cascade $$

create table `dynarap_scenario_allowed`
(
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `allowedUid` bigint not null,               -- 사용자 일련번호
    `allowedAt` bigint default 0,               -- 허용일자
    constraint pk_dynarap_scenario_allowed primary key (`scenarioSeq`, `allowedUid`)
) $$

-- 시나리오 열람 불가능자
drop table if exists `dynarap_scenario_denied` cascade $$

create table `dynarap_scenario_denied`
(
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `deniedUid` bigint not null,                -- 사용자 일련번호
    `deniedAt` bigint default 0,                -- 블럭일자
    constraint pk_dynarap_scenario_allowed primary key (`scenarioSeq`, `deniedUid`)
) $$

-- 태그
drop table if exists `dynarap_tags` cascade $$

create table `dynarap_tags`
(
    `seq` bigint auto_increment not null,       -- 태그 일련번호
    `tagType` varchar(64) not null,             -- 태그 종류 (scenario, flight_data, buffet_sb, buffet_bin)
    `tagName` varchar(32) not null,             -- 태그 이름
    `tagRefSeq` bigint not null,                -- 태그 소유 항목 일련번호
    `tagOrder` tinyint default 0,               -- 태그 순서
    `taggedAt` bigint default 0,                -- 태깅 날짜
    constraint pk_dynarap_tags primary key (`seq`)
) $$

-- 프리셋
drop table if exists `dynarap_preset` cascade $$

create table `dynarap_preset`
(
    `seq` bigint auto_increment not null,       -- 프리셋 일련번호
    `presetGroupSeq` bigint not null,           -- 프리셋 이력그룹 일련번호
    `presetName` varchar(64) not null,          -- 프리셋 이름
    `presetType` varchar(32) not null,          -- 프리셋 종류 (flight, parameter, etc)
    `createdAt` bigint default 0,               -- 생성일자
    `appliedAt` bigint default 0,               -- 적용일자
    `appliedEndAt` bigint default 0,            -- 적용종료일자.
    `creatorUid` bigint default 0,              -- 최초 작성자
    `lastModifiedBy` bigint default 0,          -- 마지막 수정자
    constraint pk_dynarap_preset primary key (`seq`)
) $$

-- 프리셋 파라미터 타입의 경우
drop table if exists `dynarap_preset_parameter` cascade $$

create table `dynarap_preset_parameter`
(
    `seq` bigint auto_increment not null,       -- 파라미터 일련번호
    `presetSeq` bigint not null,                -- 프리셋 일련번호
    `presetGroupSeq` bigint not null,           -- 프리셋 이력그룹 일련번호
    `paramName` varchar(64) not null,           -- 파라미터 이름
    `paramType` varchar(32) not null,           -- 파라미터 타입 (flight, common, etc)
    `paramDomainFrom` double default 0.0,       -- 파라미터 도메인 시작값 (임계최저)
    `paramDomainTo` double default 0.0,         -- 파라미터 도메인 종료값 (임계최대)
    `paramSpecified` double default 0.0,        -- 파라미터 지정 고정 값
    `paramUnit` varchar(32),                    -- 파라미터 수치의 단위
    `definedAt` bigint default 0,               -- 생성일자
    constraint pk_dynarap_preset_parameter primary key (`seq`)
) $$

-- 프리셋 파라미터 타입의 경우
drop table if exists `dynarap_parameter` cascade $$

create table `dynarap_parameter`
(
    `seq` bigint auto_increment not null,       -- 파라미터 일련번호
    `paramName` varchar(64) not null,           -- 파라미터 이름
    `paramGroupName` varchar(64) not null,      -- 파라미터 그룹 이름 (MATH, AOS, Q, 등)
    `paramType` varchar(32) not null,           -- 파라미터 타입 (flight, common, etc)
    `paramDomainFrom` double default 0.0,       -- 파라미터 도메인 시작값 (임계최저)
    `paramDomainTo` double default 0.0,         -- 파라미터 도메인 종료값 (임계최대)
    `paramSpecified` double default 0.0,        -- 파라미터 지정 고정 값
    `paramUnit` varchar(32),                    -- 파라미터 수치의 단위
    `definedAt` bigint default 0,               -- 생성일자
    constraint pk_dynarap_parameter primary key (`seq`)
) $$

-- 비행 개체 정의
drop table if exists `dynarap_flight` cascade $$

create table `dynarap_flight`
(
    `seq` bigint auto_increment not null,       -- 비행기체 일련번호
    `flightName` varchar(128) not null,         -- 기체 이름
    `flightType` varchar(32) not null,          -- 기체 종류
    `flightNo` varchar(32),                     -- 기체 번호
    `flightDesc` varchar(1024),                 -- 기체 설명
    `registeredAt` bigint default 0,            -- 등록일자
    `registerUid` bigint default 0,             -- 등록자
    -- 기타 flight 정보 추가 입력
    constraint pk_dynarap_flight primary key (`seq`)
) $$

-- 비행 데이터 raw table 정보
drop table if exists `dynarap_flight_file` cascade $$

create table `dynarap_flight_file`
(
    `seq` bigint auto_increment not null,       -- 데이터 일련번호
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `flightSeq` bigint not null,                -- 비행기체 일련번호
    `flightDate` bigint default 0,              -- 비행기체 비행날짜
    `fileType` varchar(32) null,                -- 파일 타입 (dll, zaero, test, csv, inline)
    `fileName` varchar(256) null,               -- 파일 이름
    `fileExt` varchar(64) null,                 -- 파일 확장자
    `fileDesc` varchar(1024) null,              -- 파일 설명
    `fileSize` bigint default 0,                -- 파일 크기
    `fileRecord` bigint default 0,              -- 파일의 레코드 수
    `fileRefDate` bigint default 0,             -- 데이터 시작 날짜 값
    `uploadedAt` bigint default 0,              -- 올린날짜
    `uploaderUid` bigint default 0,             -- 올린사람 일련번호
    `presetGroupSeq` bigint default 0,          -- 프리셋 이력 그룹 일련번호
    `presetSeq` bigint default 0,               -- 프리셋 일련번호
    constraint pk_dynarap_flight_file primary key (`seq`)
) $$

-- 비행 데이터 raw table
drop table if exists `dynarap_flight_raw` cascade $$

create table `dynarap_flight_raw`
(
    `seq` bigint auto_increment not null,       -- 데이터 일련번호
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `flightFileSeq` bigint default 0,           -- 기준 파일 정보
    `presetGroupSeq` bigint not null,           -- 데이터 프리셋 이력그룹 일련번호
    `presetSeq` bigint not null,                -- 데이터 프리셋 일련번호
    `paramSeq` bigint not null,                 -- 파라미터 메타 데이터 일련번호
    `timeOffset` bigint default 0,              -- 데이터의 시간 offset (row 데이터는 동일한 시간을 가짐, 파일의 시작 시간에서의 offset 처리)
    `timestamp` bigint default 0,               -- 절대 기준 시간으로 변환 값 (계산처리)
    `paramValue` double default 0.0,            -- 파라미터 실제 값. (모든 데이터는 더블값인가?)
    constraint pk_dynarap_flight_raw primary key (`seq`)
) $$

-- 비행 데이터 raw table (VIEW)
-- TODO : 비행 데이터 view 를 만들거나 redis에 비행 데이터 raw 테이블을 올림.
-- {"timeOffset": 0, "rows": [ {"<sensorName>": <sensorValue>, ... } ]}
-- hashOps.put("dynarap.raw", "<data_row_seq>", "[{"<sensorName>":<sensorValue>, ...}]");
-- zsetOps.rangeByScore("z.dynarap.raw", "<data_row_seq>", <timestamp score>);

-- 비행 데이터 분할 저장.
drop table if exists `dynarap_flight_part` cascade $$

create table `dynarap_flight_part`
(
    `seq` bigint auto_increment not null,       -- 비행데이터 분할 일련번호
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `flightFileSeq` bigint default 0,           -- 기준 파일 정보
    `partName` varchar(128) not null,           -- 비행 분할 이름
    `partStartAt` bigint default 0,             -- 비행 데이터 분할 시작시간 (offset)
    `partEndAt` bigint default 0,               -- 비행 데이터 분할 종료시간 (offset)
    `createdAt` bigint default 0,               -- 분할 저장 일저
    constraint pk_dynarap_flight_part primary key (`seq`)
) $$

-- 비행 데이터 raw table
drop table if exists `dynarap_flight_part_raw` cascade $$

create table `dynarap_flight_part_raw`
(
    `seq` bigint auto_increment not null,       -- 데이터 일련번호
    `scenarioSeq` bigint not null,              -- 시나리오 일련번호
    `flightFileSeq` bigint default 0,           -- 기준 파일 정보
    `partSeq` bigint default 0,                 -- 비행 데이터 분할 일련번호
    `presetGroupSeq` bigint not null,           -- 데이터 프리셋 이력그룹 일련번호
    `presetSeq` bigint not null,                -- 데이터 프리셋 일련번호
    `paramSeq` bigint not null,                 -- 파라미터 메타 데이터 일련번호
    `timeOffset` bigint default 0,              -- 데이터의 시간 offset (row 데이터는 동일한 시간을 가짐, 파일의 시작 시간에서의 offset 처리)
    `timestamp` bigint default 0,               -- 절대 기준 시간으로 변환 값 (계산처리)
    `paramValue` double default 0.0,            -- 파라미터 실제 값. (모든 데이터는 더블값인가?)
    constraint pk_dynarap_flight_part_raw primary key (`seq`)
) $$

-- 비행 데이터 part raw table (VIEW)
-- TODO : 비행 분할 데이터 view 를 만들거나 redis에 비행 분할 데이터 raw 테이블을 올림.
-- {"timeOffset": 0, "rows": [ {"<sensorName>": <sensorValue>, ... } ]}
-- hashOps.put("dynarap.raw.part<partSeq>", "<data_row_seq>", "[{"<sensorName>":<sensorValue>, ...}]");
-- zsetOps.rangeByScore("z.dynarap.raw.part<partSeq>", "<data_row_seq>", <timestamp score>);



delimiter ;

