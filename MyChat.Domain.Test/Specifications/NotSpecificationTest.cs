namespace MindLink.Recruitment.MyChat.Domain.Test.Specifications
{
    using Domain.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NotSpecificationTest
    {
        private Mock<ISpecification<object>> _alwaysSatisfied;
        private Mock<ISpecification<object>> _neverSatisfied;

        [TestInitialize]
        public void Setup()
        {
            _alwaysSatisfied = new Mock<ISpecification<object>>();
            _alwaysSatisfied.Setup(s => s.IsSatisfiedBy(It.IsAny<object>())).Returns(true);

            _neverSatisfied = new Mock<ISpecification<object>>();
            _neverSatisfied.Setup(s => s.IsSatisfiedBy(It.IsAny<object>())).Returns(false);
        }

        [TestMethod]
        public void It_must_be_satisfied_when_underlying_specification_is_not_satisfied()
        {
            var spec = new NotSpecification<object>(_neverSatisfied.Object);
            Assert.IsTrue(spec.IsSatisfiedBy(new { }));
        }

        [TestMethod]
        public void It_must_not_be_satisfied_when_underlying_specification_is_satisfied()
        {
            var spec = new NotSpecification<object>(_alwaysSatisfied.Object);
            Assert.IsFalse(spec.IsSatisfiedBy(new { }));
        }
    }
}
