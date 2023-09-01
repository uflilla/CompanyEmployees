﻿using Shared.DataTransferObjects;

namespace Services.Contracts;

public interface IEmployeeService
{
  IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
  EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
  EmployeeDto CreateEmployeeForCompany(Guid id, EmployeeForCreationDto employeeForCreation, bool trackChanges);
}