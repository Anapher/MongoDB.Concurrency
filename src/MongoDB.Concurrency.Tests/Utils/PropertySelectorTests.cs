using MongoDB.Concurrency.Utils;
using Xunit;

namespace MongoDB.Concurrency.Tests.Utils
{
    public class PropertySelectorTests
    {
        [Fact]
        public void GetValue_ValidProperty_ReturnValue()
        {
            const int value = 34;

            // arrange
            var x = new TestClass {TestProp = value};
            var selector = new PropertySelector<TestClass, int>(x => x.TestProp);

            // act
            var result = selector.GetValue(x);

            // assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void SetValue_PropertyWithPublicSetter_ShouldSetValue()
        {
            const int value = 34;

            // arrange
            var x = new TestClass();
            var selector = new PropertySelector<TestClass, int>(x => x.TestProp);

            // act
            selector.SetValue(x, value);

            // assert
            Assert.Equal(value, x.TestProp);
        }

        [Fact]
        public void SetValue_PropertyWithPrivateSetter_ShouldSetValue()
        {
            const int value = 34;

            // arrange
            var x = new TestClass();
            var selector = new PropertySelector<TestClass, int>(x => x.TestProp2);

            // act
            selector.SetValue(x, value);

            // assert
            Assert.Equal(value, x.TestProp2);
        }

        [Fact]
        public void SetValue_PropertyWithInitSetter_ShouldSetValue()
        {
            const int value = 34;

            // arrange
            var x = new TestClass();
            var selector = new PropertySelector<TestClass, int>(x => x.TestProp3);

            // act
            selector.SetValue(x, value);

            // assert
            Assert.Equal(value, x.TestProp3);
        }

        public class TestClass
        {
            public int TestProp { get; set; }

            public int TestProp2 { get; private set; }

            public int TestProp3 { get; init; }

            public void SetProp2(int val)
            {
                TestProp2 = val;
            }
        }
    }
}
