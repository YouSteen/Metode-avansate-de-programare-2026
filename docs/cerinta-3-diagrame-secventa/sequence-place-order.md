# Flux 1: Plasarea unei comenzi

Diagrama de secventa pentru fluxul **Place Order** (starea refactorizata — `OrderManagement.After`).

```mermaid
sequenceDiagram
    participant C as Client
    participant OC as OrderController
    participant OS as OrderService
    participant SS as StockService
    participant PS as PaymentService
    participant RE as OrderRepository
    participant EM as EmailService

    C->>OC: POST /orders(customer, items)
    OC->>OS: PlaceOrder(customerName, email, items)

    OS->>SS: HasStock(items)
    alt stoc disponibil
        SS-->>OS: true
        OS->>PS: ProcessPayment(email, total)
        alt plata reusita
            PS-->>OS: PaymentResult(success)
            OS->>SS: Reserve(items)
            OS->>OS: order.Confirm()
            OS->>RE: Save(order)
            opt trimitere confirmare
                OS->>EM: SendOrderConfirmation(order)
            end
            OS-->>OC: orderId
            OC-->>C: 201 Created
        else plata esuata
            PS-->>OS: PaymentResult(failed)
            OS-->>OC: throw InvalidOperationException
            OC-->>C: 402 Payment Required
        end
    else stoc insuficient
        SS-->>OS: false
        OS-->>OC: throw OutOfStockException
        OC-->>C: 409 Conflict
    end
```

## Observatii

- Blocul `alt` separa ramura de succes de cea cu stoc insuficient.
- Blocul `opt` marcheaza emailul de confirmare ca pas optional (poate esua fara a anula comanda, in variante viitoare).
- In varianta **Before**, toate apelurile sunt inline in `OrderManager` (fara straturi separate).
