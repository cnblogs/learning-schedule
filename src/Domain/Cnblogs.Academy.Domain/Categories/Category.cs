using System;
using System.ComponentModel.DataAnnotations;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Categories
{
    public class Category : IAggregateRoot
    {
        private Category()
        {
        }

        public Category(string name, long parentId)
        {
            Name = name;
            ParentId = parentId;
        }

        public long Id { get; private set; }

        [MaxLength(20)]
        public string Name { get; private set; }

        public long ParentId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }

        public bool Deleted { get; private set; }

        public void Rename(string name)
        {
            Name = name;
        }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
