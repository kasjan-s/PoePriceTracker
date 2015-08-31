CREATE TABLE [dbo].[Items] (
    [Id]          NVARCHAR (50) NOT NULL,
    [Name]        NVARCHAR (80) NOT NULL,
    [Amount]      INT           NOT NULL,
    [MaxPrice]    FLOAT (53)    NOT NULL,
    [MinPrice]    FLOAT (53)    NOT NULL,
    [MedianPrice] FLOAT (53)    NOT NULL,
    [MeanPrice]   FLOAT (53)    NOT NULL,
    [League]      NVARCHAR (15) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

