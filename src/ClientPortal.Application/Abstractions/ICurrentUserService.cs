namespace ClientPortal.Application.Abstractions;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? ClientId { get; }
}
