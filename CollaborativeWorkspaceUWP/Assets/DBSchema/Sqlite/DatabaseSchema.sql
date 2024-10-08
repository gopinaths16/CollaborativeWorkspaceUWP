CREATE TABLE CW_USER_DETAILS(ID INTEGER PRIMARY KEY AUTOINCREMENT, USERNAME varchar(50), PASSWORD varchar(20), DISPLAYNAME varchar(50));
CREATE TABLE CW_PRIORITY (ID bigint, NAME varchar(50), COLOR_CODE Text, PRIMARY KEY(ID, NAME));
CREATE TABLE CW_STATUS (ID bigint, NAME varchar(50), COLOR_CODE Text, PRIMARY KEY(ID, NAME));
CREATE TABLE CW_ORGANIZATION_DETAILS(ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, OWNERID bigint, FOREIGN KEY(OWNERID) REFERENCES CW_USER_DETAILS(ID));
CREATE TABLE CW_TEAMSPACE_DETAILS(ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, ORGANIZATIONID bigint, OWNERID bigint, FOREIGN KEY(ORGANIZATIONID) REFERENCES CW_ORGANIZATION_DETAILS(ID), FOREIGN KEY(OWNERID) REFERENCES CW_USER_DETAILS(ID));
CREATE TABLE CW_PROJECT_DETAILS (ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, STATUS bigint, PRIORITY bigint, TEAMSPACEID bigint, OWNERID bigint, FOREIGN KEY(STATUS) REFERENCES CW_STATUS(ID), FOREIGN KEY(PRIORITY) REFERENCES CW_PRIORITY(ID), FOREIGN KEY(OWNERID) REFERENCES CW_USER_DETAILS(ID));
CREATE TABLE CW_GROUP_DETAILS (ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, BOARD_GROUP_ID bigint, PROJECT_ID bigint, IS_BOARD_GROUP BOOLEAN, FOREIGN KEY(PROJECT_ID) REFERENCES CW_PROJECT_DETAILS(ID));
CREATE TABLE CW_TASK_DETAILS (ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, DESCRIPTION Text, STATUS bigint, PRIORITY bigint, PROJECTID bigint, OWNERID bigint, ASSIGNEEID bigint, PARENT_TASK_ID bigint, MODIFIED_TIME DATETIME, TASK_ORDER INTEGER, DUE_DATE DATETIME, GROUP_ID bigint, FOREIGN KEY(PROJECTID) REFERENCES CW_PROJECT_DETAILS(ID), FOREIGN KEY(STATUS) REFERENCES CW_STATUS(ID), FOREIGN KEY(PRIORITY) REFERENCES CW_PRIORITY(ID), FOREIGN KEY(OWNERID) REFERENCES CW_USER_DETAILS(ID), FOREIGN KEY(ASSIGNEEID) REFERENCES CW_USER_DETAILS(ID), FOREIGN KEY(GROUP_ID) REFERENCES CW_GROUP_DETAILS(ID));
CREATE TABLE CW_COMMENT_DETAILS(ID INTEGER PRIMARY KEY AUTOINCREMENT, MESSAGE Text, TASK_ID bigint, OWNER_ID bigint, FOREIGN KEY(TASK_ID) REFERENCES CW_TASK_DETAILS(ID), FOREIGN KEY(OWNER_ID) REFERENCES CW_USER_DETAILS(ID));
CREATE TABLE CW_ATTACHMENT_DETAILS (ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME Text, PATH Text, TYPE INTEGER, TASK_ID bigint, COMMENT_ID bigint, ADDED_TIME DATETIME DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY(TASK_ID) REFERENCES CW_TASK_DETAILS(ID), FOREIGN KEY(COMMENT_ID) REFERENCES CW_COMMENT_DETAILS(ID));

INSERT INTO CW_PRIORITY (ID, NAME, COLOR_CODE) VALUES(1, 'Low', '#246fe0');
INSERT INTO CW_PRIORITY (ID, NAME, COLOR_CODE) VALUES(2, 'Medium', '#eb8909');
INSERT INTO CW_PRIORITY (ID, NAME, COLOR_CODE) VALUES(3, 'High', '#d1453b');

INSERT INTO CW_STATUS (ID, NAME, COLOR_CODE) VALUES(1, 'Planning', '#ff0000');
INSERT INTO CW_STATUS (ID, NAME, COLOR_CODE) VALUES(2, 'In Progress', '#ff0000');
INSERT INTO CW_STATUS (ID, NAME, COLOR_CODE) VALUES(3, 'Completed', '#80cb80');