﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTwo.Data.Interfaces;
using TaskTwo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskTwo.Data.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(IApplicationContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsWithEmployeesAsync()
        {    
            return await Db.Subjects
                .Select(s => new Subject
                {
                    Id = s.Id,
                    Name = s.Name,
                    Assignments = s.Assignments.Select(es => new EmployeeSubject
                    {
                        Employee = new Employee
                        {
                            Id = es.Employee.Id,
                            FirstName = es.Employee.FirstName,
                            SecondName = es.Employee.SecondName,
                            SurName = es.Employee.SurName,
                            Email = es.Employee.Email,
                            Phones = es.Employee.Phones.Select(p => new Phone 
                            { 
                                Id = p.Id, 
                                Number = p.Number 
                            }).ToList()
                        }
                    }).ToList()
                })
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Subject> GetSubjectWithEmployeesAsync(string name)
        {
            return await Db.Subjects
                .Select(s => new Subject
                {
                    Id = s.Id,
                    Name = s.Name,
                    Assignments = s.Assignments.Select(es => new EmployeeSubject { Employee = new Employee 
                    {
                        Id = es.Employee.Id,
                        FirstName = es.Employee.FirstName,
                        SecondName = es.Employee.SecondName,
                        SurName = es.Employee.SurName,
                        Email = es.Employee.Email,
                        Phones = es.Employee.Phones.Select(p => new Phone { Id = p.Id, Number = p.Number }).ToList()
                    }}).ToList()
                })
                .FirstOrDefaultAsync(s => s.Name == name);
        }            

        public async Task<Subject> GetDetailsAsync(int id)
        {
            return await Db.Subjects
                .Include(s => s.Assignments)
                .ThenInclude(es => es.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);
        }            

        public override async Task<IEnumerable<Subject>> SearchAsync(string searchString)
        {
            return await Db.Subjects
                .Where(e => e.Name.Contains(searchString))
                .ToListAsync();
        }            
        
        public bool ExistsName(string name)
        {
            return Db.Subjects
                .Any(s => s.Name == name);
        }            
    }
}