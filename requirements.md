# Domínio do FactoryManager — Anotações

> Isso não é uma especificação formal — são anotações de domínio pra guiar o desenvolvimento incremental do projeto. Os campos exatos de cada entidade (principalmente `Recipe`) ainda vão ser definidos quando esse módulo for implementado.

---

## Visão geral

O FactoryManager simula uma fábrica onde o usuário extrai/possui **recursos**, transforma esses recursos em **produtos** através de **receitas**, usando **máquinas** específicas para cada etapa de produção.

Fluxo geral: `Recurso` → (via `Receita`, numa `Máquina`) → `Produto`

---

## Entidades

| Entidade | Propósito |
|---|---|
| `Machine` | Executa a conversão de recursos/produtos em outro produto. Tem status (Offline/Running/Maintenance), capacidade de produção, consumo de energia e controle de manutenção. **Já implementado.** |
| `Resource` | Matéria-prima bruta, obtida diretamente (não fabricada). Ex: minério, carvão, pedra. |
| `Product` | Item fabricado a partir de recursos ou outros produtos, através de uma receita. Ex: chapa de ferro, engrenagem. |
| `Recipe` | Define como um produto é fabricado: quais insumos (recursos e/ou produtos) são consumidos, em que quantidade, em quanto tempo, e possivelmente em qual máquina. |
| `User` | Possui dinheiro, recursos e produtos (inventário). Compra recursos, vende produtos, ou usa uns e outros como insumo de novas receitas. |

---

## Recursos disponíveis (matéria-prima bruta)

- Minério de Ferro
- Minério de Cobre
- Carvão
- Pedra

## Produtos (fabricados)

- Chapa de Ferro
- Chapa de Cobre
- Engrenagem
- Fio

## Máquinas

- Fornalha
- Montadora

---

## Receitas conhecidas (fluxo inicial)

Anotação livre das conversões pensadas até agora — os campos reais de `Recipe` (quantidade de cada insumo, tempo de produção, máquina responsável) serão definidos quando esse módulo for implementado.

| Insumos | Produto resultante |
|---|---|
| 3x Minério de Ferro | 2x Chapa de Ferro |
| 2x Chapa de Ferro | 5x Engrenagens |
| 2x Minério de Cobre | 1x Chapa de Cobre |
| 1x Chapa de Cobre | 2x Fios |

## Requisitos de construção das máquinas

| Máquina | Custo de construção |
|---|---|
| Fornalha | 10x Pedra |
| Montadora | 10x Chapa de Ferro + 2x Chapa de Cobre + 4x Fios |

---

## Fluxo de exemplo (cenário ponta a ponta)

1. Usuário possui recursos brutos (ex: Minério de Ferro) em seu inventário.
2. Usuário constrói uma máquina (ex: Fornalha), consumindo recursos do próprio inventário (ex: 10x Pedra).
3. Usuário aciona uma receita numa máquina compatível — a máquina consome os insumos da receita e, após o tempo de produção, gera o produto resultante.
4. O produto fabricado entra no inventário do usuário, podendo ser usado como insumo de uma nova receita (ex: Chapa de Ferro vira Engrenagem) ou mantido/vendido.

---