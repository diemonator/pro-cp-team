﻿CREATE TABLE [dbo].[PopulationTable] (
    [City]            VARCHAR (50) NOT NULL,
    [Age0_17]         FLOAT (53)   NOT NULL,
    [Age18_34]        FLOAT (53)   NOT NULL,
    [Age35_54]        FLOAT (53)   NOT NULL,
    [Age55_Up]        FLOAT (53)   NOT NULL,
    [AverageAge]      FLOAT (53)   NOT NULL,
    [BirthRate]       FLOAT (53)   NOT NULL,
    [DeathRate]       FLOAT (53)   NOT NULL,
    [EmigrationRate]  FLOAT (53)   NOT NULL,
    [ImmigrationRate] FLOAT (53)   NOT NULL,
    [FemaleRate]      FLOAT (53)   NOT NULL,
    [MaleRate]        FLOAT (53)   NOT NULL,
    [GrowthRate]      FLOAT (53)   NOT NULL,
    [Latitude]        FLOAT (53)   NOT NULL,
    [Longitude]       FLOAT (53)   NOT NULL,
    [PopulationNr]    FLOAT (53)   NOT NULL,
    [Year]            INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([City] ASC, [Year] ASC)
);
