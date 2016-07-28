namespace MindLink.Recruitment.MyChat.Domain.Test.Specifications
{
    using Domain.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SpecificationTest
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
        public void It_must_generate_and_specification()
        {
            ISpecification<object> spec = _neverSatisfied.Object.And(_alwaysSatisfied.Object);
            Assert.IsInstanceOfType(spec, typeof(AndSpecification<object>));
        }

        [TestMethod]
        public void It_must_generate_or_specification()
        {
            ISpecification<object> spec = _neverSatisfied.Object.Or(_alwaysSatisfied.Object);
            Assert.IsInstanceOfType(spec, typeof(OrSpecification<object>));
        }

        [TestMethod]
        public void It_must_generate_not_specification()
        {
            ISpecification<object> spec = _alwaysSatisfied.Object.Not();
            Assert.IsInstanceOfType(spec, typeof(NotSpecification<object>));
        }
    }
}
