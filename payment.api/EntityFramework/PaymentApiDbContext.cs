using Microsoft.EntityFrameworkCore;

namespace payment.api.EntityFramework {
    public class PaymentApiDbContext : DbContext {
        public PaymentApiDbContext (DbContextOptions<PaymentApiDbContext> options) : base (options) {

        }
    }
}