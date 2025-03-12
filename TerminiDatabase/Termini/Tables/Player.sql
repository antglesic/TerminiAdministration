CREATE TABLE [Termini].[Player] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Active]      BIT           CONSTRAINT [DF_Termini_Player_Active] DEFAULT ((1)) NOT NULL,
    [DateCreated] DATETIME      CONSTRAINT [DF_Termini_Player_DateCreated] DEFAULT (getdate()) NOT NULL,
    [Name]        VARCHAR (255) NULL,
    [Surname]     VARCHAR (255) NULL,
    [Foot]        VARCHAR (5)   NULL,
    [Sex]         VARCHAR (6)   NULL,
    CONSTRAINT [PK_Termini_Player_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

