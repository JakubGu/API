using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using API.Attributes;

namespace API.Tests.UnitTests
{
    public class PasswordComplexityAttributeTests
    {
        private readonly PasswordComplexityAttribute _passwordComplexityAttribute;

        public PasswordComplexityAttributeTests()
        {
            _passwordComplexityAttribute = new PasswordComplexityAttribute();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_ReturnFalse_When_PasswordIsNullOrEmpty(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetInvalidPasswordLengthData()
        {
            yield return new object[] { "a" };
            yield return new object[] { "ab" };
            yield return new object[] { "abc" };
            yield return new object[] { new string('a', 33) };
        }

        [Theory]
        [MemberData(nameof(GetInvalidPasswordLengthData))]
        public void Should_ReturnFalse_When_PasswordLengthIsInvalid(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        public void Should_ReturnFalse_When_PasswordDoesNotContainDigit(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("ABCD1")]
        public void Should_ReturnFalse_When_PasswordDoesNotContainLowercaseLetter(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("abcd1")]
        [InlineData("12345")]
        public void Should_ReturnFalse_When_PasswordDoesNotContainUppercaseLetter(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("Abcd1")]
        [InlineData("ABCD1a")]
        public void Should_ReturnTrue_When_PasswordIsValid(string password)
        {
            var result = _passwordComplexityAttribute.IsValid(password);
            result.Should().BeTrue();
        }
    }
}