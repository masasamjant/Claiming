namespace Masasamjant.Claiming.SqlServer
{
    internal sealed class EntityClaimSchema
    {
        public EntityClaimSchema(string schemaName, string tableName, int ownerIdentifierMaxLength, int typeNameMaxLength, int instanceIdentifierMaxLength)
        {
            SchemaName = schemaName;
            TableName = tableName;
            OwnerIdentifierMaxLength = ownerIdentifierMaxLength;
            TypeNameMaxLength = typeNameMaxLength;
            InstanceIdentifierMaxLength = instanceIdentifierMaxLength;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public int OwnerIdentifierMaxLength { get; }

        public int TypeNameMaxLength { get; }

        public int InstanceIdentifierMaxLength { get; }

        public static EntityClaimSchema Default { get; } = new EntityClaimSchema("Claims", "EntityClaim", 128, 256, 256);
    }
}
