﻿using System.Runtime.InteropServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
  [Route("api/companies/{companyId}/employees")]
  [ApiController]
  public class EmployeesController : ControllerBase
  {
    private readonly IServiceManager _service;
    public EmployeesController(IServiceManager service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
      var employees =await _service.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
      return Ok(employees);
    }

    [HttpGet("{id:guid}",Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
      var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, false);
      return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
      /* Replaced with validation filter attribute
       if (employee is null)
        return BadRequest("EmployeeForCreationDto object is null");
      //ModelState.AddModelError("custom error key", "string errorMessage");
      //ModelState.AddModelError("custom error key", "string errorMessage 2");
      if (!ModelState.IsValid)
        return UnprocessableEntity(ModelState);*/

      var employeeToReturn =await 
        _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
      
      return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
      await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId,id, false);
      return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
      await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId,id,employee,false,true);
      return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
      if (patchDoc is null)
        return BadRequest("patchDoc object sent from client is null.");
      var result =await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false,
        empTrackChanges: true);
      patchDoc.ApplyTo(result.employeeToPatch,ModelState);

      

      TryValidateModel(result.employeeToPatch);
      if (!ModelState.IsValid)
        return UnprocessableEntity(ModelState);

      await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
      return NoContent();
    }
  }
}
