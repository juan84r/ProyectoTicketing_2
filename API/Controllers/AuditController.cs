using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/v1/audit-logs")]
public class AuditController : ControllerBase 
{
    private readonly IAuditRepository _auditRepository;

    public AuditController(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await _auditRepository.GetAllAsync();
        return Ok(logs);
    }
}