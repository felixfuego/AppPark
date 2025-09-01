using Park.Web.Models;

namespace Park.Web.Services;

public interface IVisitorService : IApiService<Visitor, CreateVisitor, UpdateVisitor>
{
    Task<Visitor?> GetByEmailAsync(string email);
    Task<Visitor?> GetByDocumentAsync(string documentType, string documentNumber);
}
