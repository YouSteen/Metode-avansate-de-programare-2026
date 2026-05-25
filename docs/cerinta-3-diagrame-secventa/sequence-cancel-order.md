# Flux 2: Anularea unei comenzi

Diagrama de secventa pentru fluxul **Cancel Order**, inclusiv ramura in care anularea nu este permisa.

```mermaid
sequenceDiagram
    participant C as Client
    participant OC as OrderController
    participant OS as OrderService
    participant RE as OrderRepository
    participant SS as StockService
    participant EM as EmailService

    C->>OC: DELETE /orders/{orderId}
    OC->>OS: CancelOrder(orderId)

    OS->>RE: FindById(orderId)
    RE-->>OS: Order

    alt comanda exista
        alt status != Shipped
            OS->>OS: order.Cancel()
            OS->>SS: Release(order.Items)
            OS->>RE: Save(order)
            opt notificare client
                OS->>EM: SendOrderCancellation(order)
            end
            OS-->>OC: true
            OC-->>C: 200 OK
        else comanda deja expediata
            OS-->>OC: false
            OC-->>C: 403 Forbidden (anulare interzisa)
        end
    else comanda inexistenta
        OS-->>OC: throw NotFoundException
        OC-->>C: 404 Not Found
    end
```

## Observatii

- Ramura `alt` pentru `Shipped` reflecta regula de business: comanda expediata nu se anuleaza.
- `Release` pe stoc restituie cantitatile rezervate la plasare.
- In varianta **Before**, `OrderManager.CancelOrder` combina validare, DB, stoc si email intr-o singura metoda.
