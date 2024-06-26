﻿using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.EventStore.Entities;


namespace CleanArchitecture.Infrastructure.Common.Data.EventStore
{
    public class EventContext : DbContext
    {

        public EventContext() { }

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
