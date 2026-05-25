# Cerinta 3 — Diagrame de secventa (2 fluxuri)

## Enunt (din laborator)

| Flux | Descriere |
|------|-----------|
| **Flux 1** | Plasarea unei comenzi — de la HTTP la confirmare + email |
| **Flux 2** | Anularea unei comenzi — inclusiv cazul **anulare interzisa** (comanda expediată) |

Folositi blocuri **`alt`** (ramuri alternative) si **`opt`** (comportament optional).

Tool recomandat: **Mermaid** in fisiere `.md` (vizualizare pe GitHub / VS Code).

---

## Livrabil 3a — Flux 1: Plasare comanda

**Fisier:** [sequence-place-order.md](sequence-place-order.md)

Diagrama (preview):

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
            OS->>RE: Save(order)
            opt trimitere confirmare
                OS->>EM: SendOrderConfirmation(order)
            end
            OS-->>OC: orderId
            OC-->>C: 201 Created
        else plata esuata
            OC-->>C: 402 Payment Required
        end
    else stoc insuficient
        OC-->>C: 409 Conflict
    end
```

---

## Livrabil 3b — Flux 2: Anulare comanda

**Fisier:** [sequence-cancel-order.md](sequence-cancel-order.md)

Diagrama (preview):

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
    alt comanda exista
        alt status != Shipped
            OS->>OS: order.Cancel()
            OS->>SS: Release(order.Items)
            OS->>RE: Save(order)
            opt notificare client
                OS->>EM: SendOrderCancellation(order)
            end
            OC-->>C: 200 OK
        else comanda deja expediata
            OC-->>C: 403 Forbidden
        end
    else comanda inexistenta
        OC-->>C: 404 Not Found
    end
```

---

## Checklist

| Element | Flux 1 | Flux 2 |
|---------|--------|--------|
| Mesaje sincron | da | da |
| bloc `alt` | stoc / plata | exista comanda / Shipped |
| bloc `opt` | email confirmare | email anulare |
| Ramura esec | stoc, plata | 403, 404 |

