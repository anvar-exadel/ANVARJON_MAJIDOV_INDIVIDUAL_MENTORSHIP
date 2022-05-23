using DatabaseAccess;
using Shared.dtos.mailDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.services
{
    public class MailService
    {
        private readonly AppDbContext _context;

        public MailService(AppDbContext context)
        {
            _context = context;
        }

        public void Subscribe(SubsribeUserDto subsribe)
        {
            return;
        }
    }
}
