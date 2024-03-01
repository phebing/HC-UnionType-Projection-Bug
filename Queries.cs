using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace ProjectionBug;

[ExtendObjectType(OperationTypeNames.Query)]
public class Queries
{
    public class Base
    {
        public string C { get; set; }
    }

    public class ChildA : Base
    {
        public string A { get; set; }
    }

    public class ChildB : Base
    {
        public string B { get; set; }
    }

    public class ChildAType : ObjectType<ChildA>
    {
        protected override void Configure(IObjectTypeDescriptor<ChildA> descriptor)
        {
            descriptor.Field(_ => _.A).Type<NonNullType<StringType>>();
        }
    }

    public class ChildBType : ObjectType<ChildB>
    {
        protected override void Configure(IObjectTypeDescriptor<ChildB> descriptor)
        {
            descriptor.Field(_ => _.B).Type<NonNullType<StringType>>();
        }
    }

    public class UnionTestType : UnionType
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            base.Configure(descriptor);
            descriptor.Name("UnionTestType");
            descriptor.Type<ChildAType>();
            descriptor.Type<ChildBType>();
        }
    }

    [UseProjection]
    [GraphQLType(typeof(ListType<UnionTestType>))]
    public IQueryable<Base> GetUnionTest()
    {
        var types = new List<Base>();
        return types.AsQueryable();
    }
}