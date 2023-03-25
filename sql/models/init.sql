
--CREATE TABLE "PatientDatas"
--(
--    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
--    "PatientId" bigint NOT NULL,
--    "InfluenceId" bigint NOT NULL,
--    "Timestamp" date NOT NULL,
--    CONSTRAINT "PatientDatas_pkey" PRIMARY KEY ("Id")
--);

CREATE TABLE "Models"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "FileName" text COLLATE pg_catalog."default" NOT NULL,
    "Accuracy" integer NOT NULL,
    "Version" text NOT NULL,
    "InputParamsCount" integer NOT NULL,
    "OutputParamsCount" integer NOT NULL,
    "ParamsNames" text[] COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Models_pkey" PRIMARY KEY ("Id", "Version")
)


