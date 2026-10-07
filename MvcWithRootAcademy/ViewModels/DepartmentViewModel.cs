using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Demo.pl.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Requierd")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Requierd")]
        public string Name { get; set; }

        [DisplayName("Date Of Creaction")]
        public DateTime DateOfCreaction { get; set; }


    }
}
