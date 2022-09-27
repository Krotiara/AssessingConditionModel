
CREATE TABLE "PatientDatas"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "PatientId" bigint NOT NULL,
    "InfluenceId" bigint NOT NULL,
    "Timestamp" date NOT NULL,
    CONSTRAINT "PatientDatas_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE "PatientParameters"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "PatientId" bigint NOT NULL,
    "Timestamp" date NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Value" text COLLATE pg_catalog."default" NOT NULL,
    "DynamicValue" text COLLATE pg_catalog."default",
    "PositiveDynamicCoef" bigint NOT NULL,
    "PatientDataId" bigint NOT NULL,
    CONSTRAINT "PatientParameters_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE "Patients"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "MedicalHistoryNumber" bigint NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Birthday" date NOT NULL,
    CONSTRAINT "Patients_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE "Influences"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "PatientId" bigint NOT NULL,
    "StartTimestamp" date NOT NULL,
    "EndTimestamp" date NOT NULL,
    "InfluenceType" int NOT NULL,
    "MedicineName" text COLLATE pg_catalog."default" NOT NULL
);

