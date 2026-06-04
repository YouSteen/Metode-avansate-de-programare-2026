const state = { orders: [], selectedId: null, products: [] };

const ACTIONS = {
  Pending: ["pay", "cancel"],
  Confirmed: ["process", "cancel"],
  Processing: ["ship", "cancel"],
  Shipped: ["deliver"],
  Delivered: [],
  Cancelled: []
};

const FLOW = ["Pending", "Confirmed", "Processing", "Shipped", "Delivered"];

async function api(path, options = {}) {
  const res = await fetch(path, {
    headers: { "Content-Type": "application/json", ...options.headers },
    ...options
  });
  const text = await res.text();
  const data = text ? JSON.parse(text) : null;
  if (!res.ok) {
    const msg = data?.message || (data?.errors && data.errors.join(", ")) || res.statusText;
    throw new Error(msg);
  }
  return data;
}

function shortId(id) {
  const s = String(id);
  return "#" + s.slice(0, 8);
}

function showToast(msg, ok = true) {
  const el = document.getElementById("toast");
  el.textContent = msg;
  el.className = "toast " + (ok ? "ok" : "err");
  clearTimeout(showToast._t);
  showToast._t = setTimeout(() => el.classList.add("hidden"), 4000);
  el.onclick = () => el.classList.add("hidden");
}

function renderList() {
  const ul = document.getElementById("orderList");
  document.getElementById("orderCount").textContent = "· " + state.orders.length;
  ul.innerHTML = state.orders.map(o => {
    const id = o.id;
    const active = id === state.selectedId ? " active" : "";
    return `<li class="order-row${active}" data-id="${id}">
      <span>${id === state.selectedId ? "▶" : ""}</span>
      <span class="oid">${shortId(id)}</span>
      <span class="badge ${o.status}">${o.status}</span>
    </li>`;
  }).join("");
  ul.querySelectorAll(".order-row").forEach(row => {
    row.onclick = () => selectOrder(row.dataset.id);
  });
}

function renderDetails(order) {
  const panel = document.getElementById("detailsPanel");
  const items = order.items.map(i =>
    `<li>${i.productName} x${i.quantity} @ ${i.unitPrice} RON</li>`).join("");
  const idx = FLOW.indexOf(order.status);
  const nodes = FLOW.map((n, i) => {
    let cls = "state-node";
    if (i < idx) cls += " done";
    if (n === order.status) cls += " current";
    return `<span class="${cls}">${n}</span>${i < FLOW.length - 1 ? "<span>→</span>" : ""}`;
  }).join("");
  const allowed = ACTIONS[order.status] || [];
  const btns = ["pay", "process", "ship", "deliver", "cancel"].map(a => {
    const on = allowed.includes(a);
    const label = a.charAt(0).toUpperCase() + a.slice(1);
    const cls = on ? "btn enabled" : "btn" + (a === "cancel" ? " cancel-action" : "");
    return `<button type="button" class="${cls}" data-action="${a}" ${on ? "" : "disabled"}>${label}</button>`;
  }).join("");
  const hist = (order.history || []).map(h => {
    const t = new Date(h.at).toLocaleTimeString();
    return `<div class="history-row"><span>${h.fromState} → ${h.toState}</span><span class="ts">${t}</span></div>`;
  }).join("") || "<div class='muted'>Fara istoric inca.</div>";
  panel.innerHTML = `
    <div class="panel"><div class="panel-label">Order Details</div>
      <dl class="detail-grid">
        <dt>ID</dt><dd class="mono">${order.id}</dd>
        <dt>Status</dt><dd>${order.status}</dd>
        <dt>Customer</dt><dd>${order.customer.name} (${order.customer.age} ani)</dd>
        <dt>Items</dt><dd><ul>${items}</ul></dd>
        <dt>Total</dt><dd>${order.total.amount} ${order.total.currency}</dd>
        <dt>Address</dt><dd>${order.shippingAddress.street}, ${order.shippingAddress.city}</dd>
      </dl>
    </div>
    <div class="panel"><div class="panel-label">State Diagram</div><div class="state-flow">${nodes}</div></div>
    <div class="panel"><div class="panel-label">Actions</div><div class="actions">${btns}</div></div>
    <div class="panel"><div class="panel-label">History</div><div class="history">${hist}</div></div>`;
  panel.querySelectorAll("[data-action]").forEach(b => {
    if (!b.disabled) b.onclick = () => triggerAction(order.id, b.dataset.action);
  });
}

async function fetchOrders() {
  state.orders = await api("/orders");
  renderList();
  if (state.selectedId) {
    const o = state.orders.find(x => x.id === state.selectedId);
    if (o) renderDetails(o);
  }
}

async function selectOrder(id) {
  state.selectedId = id;
  const order = await api("/orders/" + id);
  renderList();
  renderDetails(order);
}

async function triggerAction(id, action) {
  try {
    const order = await api(`/orders/${id}/${action}`, { method: "POST" });
    state.selectedId = id;
    await fetchOrders();
    renderDetails(order);
    showToast(`Actiune ${action} reusita`, true);
  } catch (e) {
    showToast(e.message, false);
  }
}

function addItemRow() {
  const c = document.getElementById("itemsContainer");
  const opts = state.products.map(p =>
    `<option value="${p.productId}" data-price="${p.unitPrice}" data-age="${p.hasAgeRestriction}">${p.name} (stoc ${p.stock})</option>`).join("");
  const row = document.createElement("div");
  row.className = "item-row";
  row.innerHTML = `<select class="prod">${opts}</select>
    <input type="number" class="qty" value="1" min="1" />
    <input type="number" class="price" value="49.99" step="0.01" />
    <button type="button" class="btn small rm">X</button>`;
  row.querySelector(".rm").onclick = () => row.remove();
  row.querySelector(".prod").onchange = e => {
    const o = e.target.selectedOptions[0];
    row.querySelector(".price").value = o.dataset.price || 49.99;
  };
  c.appendChild(row);
}

function collectItems() {
  return [...document.querySelectorAll(".item-row")].map(row => {
    const sel = row.querySelector(".prod");
    const opt = sel.selectedOptions[0];
    return {
      productId: sel.value,
      productName: opt.textContent.split(" (")[0],
      quantity: parseInt(row.querySelector(".qty").value, 10),
      unitPrice: parseFloat(row.querySelector(".price").value),
      hasAgeRestriction: opt.dataset.age === "true"
    };
  });
}

async function createOrder() {
  const items = collectItems();
  const declaredTotal = items.reduce((s, i) => s + i.quantity * i.unitPrice, 0);
  const body = {
    customer: {
      id: crypto.randomUUID(),
      name: document.getElementById("custName").value,
      email: document.getElementById("custEmail").value,
      age: parseInt(document.getElementById("custAge").value, 10),
      isTrusted: document.getElementById("custTrusted").checked
    },
    shippingAddress: {
      street: document.getElementById("addrStreet").value,
      city: document.getElementById("addrCity").value,
      postalCode: document.getElementById("addrPostal").value,
      country: document.getElementById("addrCountry").value
    },
    items,
    declaredTotal
  };
  try {
    const order = await api("/orders", { method: "POST", body: JSON.stringify(body) });
    document.getElementById("modal").classList.add("hidden");
    state.selectedId = order.id;
    await fetchOrders();
    renderDetails(order);
    showToast("Comanda creata " + shortId(order.id), true);
  } catch (e) {
    showToast(e.message, false);
  }
}

document.getElementById("btnNew").onclick = () => {
  document.getElementById("itemsContainer").innerHTML = "";
  addItemRow();
  document.getElementById("modal").classList.remove("hidden");
};
document.getElementById("btnCancelModal").onclick = () =>
  document.getElementById("modal").classList.add("hidden");
document.getElementById("btnSubmitOrder").onclick = createOrder;
document.getElementById("btnAddItem").onclick = addItemRow;

(async () => {
  state.products = await api("/api/products");
  await fetchOrders();
})();
