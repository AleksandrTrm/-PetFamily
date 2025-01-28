using Microsoft.AspNetCore.Mvc;

namespace PetFamily.Shared.Framework;

[ApiController]
[Route("api/[controller]")]
public abstract class ApplicationController : ControllerBase { }