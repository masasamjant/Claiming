namespace Masasamjant.Claiming.SqlServer
{
    internal sealed class EntityClaimSchema
    {
        public EntityClaimSchema(string schemaName, string tableName, int ownerIdentifierMaxLength, int typeNameMaxLength, int instanceIdentifierMaxLength, int applicationMaxLength)
        {
            SchemaName = schemaName;
            TableName = tableName;
            OwnerIdentifierMaxLength = ownerIdentifierMaxLength;
            TypeNameMaxLength = typeNameMaxLength;
            InstanceIdentifierMaxLength = instanceIdentifierMaxLength;
            ApplicationMaxLength = applicationMaxLength;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public int OwnerIdentifierMaxLength { get; }

        public int TypeNameMaxLength { get; }

        public int InstanceIdentifierMaxLength { get; }

        public int ApplicationMaxLength { get; }

        public static EntityClaimSchema Default { get; } = new EntityClaimSchema("Claims", "EntityClaim", 128, 256, 256, 128);
    }
}
