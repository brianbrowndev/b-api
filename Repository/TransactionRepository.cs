using System.Linq;
using Microsoft.EntityFrameworkCore;
using B.API.Models;
using System.Collections.Generic;

namespace B.API.Database
{

    public class TransactionRepository
    {
        private readonly ApiDbContext _context;

        public TransactionRepository(ApiDbContext context)
        {
            _context = context;
        }

        public TransactionRecord Find(long id) 
        {
            return Include(_context.TransactionRecord).First(b => b.Id == id);
        }

        
        public IQueryable<TransactionRecord> FindAll() 
        {
            return Include(_context.TransactionRecord);
        }

        public IQueryable<TransactionRecord> Include(IQueryable<TransactionRecord> items) 
        {
            return items.Include(o => o.Bank).Include(o => o.Category).Include(o => o.User);
        }



        public IQueryable<TransactionRecord> Order(IQueryable<TransactionRecord> items, string sortName) 
        {
            // TODO: tranlsate this to a generic method using IQueryable? 
            // May not be worth it, this is clear and conscise as is
            switch(sortName) {
                case "id_asc":
                    items = items.OrderBy(o => o.Id);
                    break;
                case "id_desc":
                    items = items.OrderByDescending(o => o.Id);
                    break;
                case "date_asc":
                    items = items.OrderBy(o => o.Date);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(o => o.Date);
                    break;
                case "user_asc":
                    items = items.OrderBy(o => o.User.FirstName);
                    break;
                case "user_desc":
                    items = items.OrderByDescending(o => o.User.FirstName);
                    break;
                case "bank_asc":
                    items = items.OrderBy(o => o.Bank.Name);
                    break;
                case "bank_desc":
                    items = items.OrderByDescending(o => o.Bank.Name);
                    break;
                case "category_asc":
                    items = items.OrderBy(o => o.Category.Name);
                    break;
                case "category_desc":
                    items = items.OrderByDescending(o => o.Category.Name);
                    break;
                case "amount_asc":
                    items = items.OrderBy(o => o.Amount);
                    break;
                case "amount_desc":
                    items = items.OrderByDescending(o => o.Amount);
                    break;
                case "description_asc":
                    items = items.OrderBy(o => o.Description);
                    break;
                case "description_desc":
                    items = items.OrderByDescending(o => o.Description);
                    break;
                default:
                    break;
            }
            return items;
        }
 
        public IQueryable<TransactionRecord> Filter(IQueryable<TransactionRecord> items, string description, List<long> categories, List<long> banks, List<long> users, List<string> years)
        {
            if (!string.IsNullOrEmpty(description)) {
                items = items.Where(o => o.Description.Contains(description));
            }
            if (categories?.Any() == true) {
                items = items.Where(o => categories.Contains(o.Category.Id));
            }
            if (banks?.Any() == true) {
                items = items.Where(o => banks.Contains(o.Bank.Id));
            }
            if (users?.Any() == true) {
                items = items.Where(o => users.Contains(o.User.Id));
            }
            if (years?.Any() == true) {
                items = items.Where(b => years.Contains(b.Date.Substring(0,4)));
            }
           return items;
        }

  }
}