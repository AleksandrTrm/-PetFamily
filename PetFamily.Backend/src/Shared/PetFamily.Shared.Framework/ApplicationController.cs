using Microsoft.AspNetCore.Mvc;

namespace PetFamily.Shared.Framework;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase { }