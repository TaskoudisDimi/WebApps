﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClass
{
    public class TableNameAttribute : Attribute
    {
        string _tableName = "";

        public string TableName { get => _tableName; set => _tableName = value; }

        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DatabaseColumnAttribute : Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsEncrypted { get; set; }

        public DatabaseColumnAttribute(bool isPrimaryKey = true, bool isForeignKey = true, bool isEncrypted = false)
        {
            IsPrimaryKey = isPrimaryKey;
            IsForeignKey = isForeignKey;
            IsEncrypted = isEncrypted;
        }

    }

    public class FieldSizeAttribute : Attribute
    {
        int _Size = 0;

        public int Size { get => _Size; set => _Size = value; }

        public FieldSizeAttribute(int Size)
        {
            _Size = Size;
        }
    }

    public class BinaryFieldAttribute : Attribute
    {

    }

    public class NullableFieldAttribute : Attribute
    {
        bool _Nullable = false;

        public bool Nullable { get => _Nullable; set => _Nullable = value; }

        public NullableFieldAttribute(bool Nullable)
        {
            _Nullable = Nullable;
        }
    }

    public class PrimaryKeyAttribute : Attribute
    {

    }

    public class EncryptedAttribute : Attribute
    {

    }

    public class ImageAttribute : Attribute
    {

    }

    public class ExportAttribute : Attribute
    {
        public string name { get; set; }

        public ExportAttribute(string name = "")
        {
            this.name = name;
        }
    }

    public class UpdateExtraFieldAttribute : Attribute
    {
        string _field = "";

        public UpdateExtraFieldAttribute(string field)
        {
            this.Field = field;
        }

        public string Field { get => _field; set => _field = value; }
    }
}
