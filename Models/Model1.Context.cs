﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeSplitApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WeSplitAppDBEntities : DbContext
    {
        public WeSplitAppDBEntities()
            : base("name=WeSplitAppDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Journey> Journeys { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
    }
}
