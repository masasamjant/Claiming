CREATE SCHEMA [Claims];
GO

CREATE TABLE [Claims].[EntityClaim]
(
	[ClaimIdentifier] UNIQUEIDENTIFIER NOT NULL,
	[OwnerIdentifier] NVARCHAR(128) NOT NULL,
	[ExpiresAt] DATETIMEOFFSET(7),
	[AssemblyQualifiedTypeName] NVARCHAR(256) NOT NULL,
	[InstanceIdentifierSHA1] NVARCHAR(256) NOT NULL,
	[Application] NVARCHAR(128) NOT NULL,
	[UniqueClaimSHA1] NVARCHAR(256) NOT NULL
);
GO

ALTER TABLE [Claims].[EntityClaim]
	ADD CONSTRAINT [PK_EntityClaim_ClaimIdentifier]
	PRIMARY KEY (ClaimIdentifier);
GO

CREATE UNIQUE INDEX UIX_EntityClaim_UniqueClaim ON [Claims].[EntityClaim](UniqueClaimSHA1);
GO