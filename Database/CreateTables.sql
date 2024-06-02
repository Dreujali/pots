CREATE TABLE LID 
(
  ID                    INTEGER         GENERATED BY DEFAULT AS IDENTITY NOT NULL,
  NAME                  VARCHAR(255) NOT NULL,
  DIAMETER                FLOAT         NOT NULL,
  IMAGE                 VARCHAR(255),
  CONSTRAINT PK_LID PRIMARY KEY (ID)
);


CREATE TABLE POT 
(
  ID                    INTEGER         GENERATED BY DEFAULT AS IDENTITY NOT NULL,
  NAME                  VARCHAR(255) NOT NULL,
  DIAMETER                FLOAT         NOT NULL,
  VOLUME                  FLOAT         NOT NULL,
  IMAGE                 VARCHAR(255),
  CONSTRAINT PK_POT PRIMARY KEY (ID)
);


CREATE TABLE MATERIAL 
(
  ID                INTEGER         GENERATED BY DEFAULT AS IDENTITY NOT NULL,
  NAME              VARCHAR(255) NOT NULL,
  CONSTRAINT PK_MATERIAL PRIMARY KEY (ID)
);


CREATE TABLE LID_MATERIAL 
(
  LID_ID                   INTEGER         NOT NULL,
  MATERIAL_ID              INTEGER         NOT NULL
);


CREATE TABLE POT_MATERIAL 
(
  POT_ID                   INTEGER         NOT NULL,
  MATERIAL_ID              INTEGER         NOT NULL
);
