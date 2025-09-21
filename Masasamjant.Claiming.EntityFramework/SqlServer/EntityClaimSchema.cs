namespace Masasamjant.Claiming.SqlServer
{
    internal sealed class EntityClaimSchema
    {
        public EntityClaimSchema(string schemaName, string tableName, int ownerIdentifierMaxLength, int typeNameMaxLength, int instanceIdentifierMaxLength, int applicationMaxLength, int uniqueClaimSHA1MaxLength, string uniqueClaimSHA1Index)
        {
            SchemaName = schemaName;
            TableName = tableName;
            OwnerIdentifierMaxLength = ownerIdentifierMaxLength;
            TypeNameMaxLength = typeNameMaxLength;
            InstanceIdentifierMaxLength = instanceIdentifierMaxLength;
            ApplicationMaxLength = applicationMaxLength;
            UniqueClaimSHA1MaxLength = uniqueClaimSHA1MaxLength;
            UniqueClaimSHA1Index = uniqueClaimSHA1Index;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public int OwnerIdentifierMaxLength { get; }

        public int TypeNameMaxLength { get; }

        public int InstanceIdentifierMaxLength { get; }

        public int ApplicationMaxLength { get; }

        public int UniqueClaimSHA1MaxLength { get; }
 
        public string UniqueClaimSHA1Index { get; }

        public static EntityClaimSchema Default { get; } = new EntityClaimSchema("Claims", "EntityClaim", 128, 256, 256, 128, 256, "UIX_EntityClaim_UniqueClaim");
    }
}
