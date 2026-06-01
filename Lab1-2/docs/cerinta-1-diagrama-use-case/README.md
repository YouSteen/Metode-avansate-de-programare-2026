# Cerinta 1 — Diagrama de cazuri de utilizare

## Enunt (din laborator)

- Identificati actorii: **Client**, **Admin**, **Sistem de plata**
- Cel putin **6 cazuri de utilizare** relevante
- Relatii **«include»** si **«extend»** unde exista
- Granita sistemului trasata

## Checklist

| Element | Status |
|---------|--------|
| Actor Client | da |
| Actor Admin | da |
| Actor Sistem Plati | da |
| 6+ cazuri de utilizare | da (8) |
| «include» | da (plasare comanda) |
| «extend» | da (anulare → vizualizare) |
| Granita sistem | da |

## Livrabil — diagrama

![Diagrama use case](use-case.svg)

**Fisier:** [use-case.svg](use-case.svg) — deschide in browser sau preview Markdown.

## Cazuri de utilizare

| Caz | Actor(i) |
|-----|----------|
| Plaseaza comanda | Client |
| Anuleaza comanda | Client |
| Vizualizeaza comanda | Client |
| Actualizeaza status | Admin |
| Genereaza raport vanzari | Admin |
| Gestioneaza catalog produse | Admin |
| Verifica stoc | «include» in Plaseaza comanda |
| Proceseaza plata | «include» + Sistem Plati |
| Trimite confirmare email | «include» in Plaseaza comanda |

## Surse (editare / export PNG)

| Fisier | Tool |
|--------|------|
| [surse/use-case.puml](surse/use-case.puml) | PlantUML |
| [surse/use-case.mmd](surse/use-case.mmd) | Mermaid |
