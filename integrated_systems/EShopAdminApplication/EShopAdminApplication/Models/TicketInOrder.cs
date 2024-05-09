using System.Net.Sockets;

namespace EShopAdminApplication.Models
{
    public class TicketInOrder
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Ticket? OrderedProduct { get; set; }

        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        public int Quantity { get; set; }
    }
}
