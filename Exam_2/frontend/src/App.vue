<template>
  <div class="container">
    <h1>Chiki Coffee Machine</h1>

    <!-- Coffees available: -->
    <h2>Select the amount of coffees you desire:</h2>
    <div
      v-for="coffee in coffees"
      :key="coffee.name"
      class="coffee-row"
    >
      <div>
        <strong>{{ coffee.name }}</strong>
          {{ coffee.price }} colones
          {{ coffee.quantity }} availables
      </div>

      <input
        type="number"
        min="0"
        :max="coffee.quantity"
        v-model.number="order[coffee.name]"
        :disabled="coffee.quantity === 0"
        @input="calculateTotal()"
      />
    </div>

    <hr />

    <!-- Money input -->
    <h2>Input your money</h2>
    <div class="money-grid">
      <div v-for="d in denominations" :key="d">
        <label>{{ d }} colones</label>
        <input type="number" min="0" v-model.number="cash[d]" @input="updatePayment" />
      </div>
    </div>

    <p><strong>Total ingresado:</strong> {{ payment.totalAmount }} colones</p>

    <hr/>

    <!-- Checkout -->
    <h2>Checkout</h2>
    <p>Total: {{ total }} colones</p>

    <p v-if="payment.totalAmount < total" style="color:red;">
      Insufficent funds
    </p>

    <!-- Buy Botton -->
    <button
      @click="buyCoffee"
      :disabled="!canBuy"
      class="buy-btn"
    >
      Buy
    </button>

    <p v-if="errorMessage" class="error-msg">
      {{ errorMessage }}
    </p>

  </div>
</template>


<script setup>
import { ref, reactive, computed, onMounted } from "vue";
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:7071/api/CoffeeMachine"
});

const coffees = ref([]);
const order = reactive({});
const total = ref(0);

const denominations = [1000, 500, 100, 50, 25];

// Cash input from user:
const cash = reactive({});
denominations.forEach(d => (cash[d] = 0));

// Payment model:
const payment = reactive({
  totalAmount: 0,
  coins: [],
  bills: []
});

const result = ref(null);
const errorMessage = ref("");

async function loadCoffees() {
  try {
    const amountsResp = await api.get("/getCoffeeAmounts");
    const pricesResp = await api.get("/getCoffeePrices");

    coffees.value = Object.keys(amountsResp.data).map(name => ({
      name,
      quantity: amountsResp.data[name],
      price: pricesResp.data[name] ?? 0
    }));

  } catch (error) {
    console.error("Error loading coffee data:", error);
  }
}

onMounted(loadCoffees);

// Calculate total:
function calculateTotal() {
  total.value = coffees.value.reduce((sum, c) => {
    const coffeeAmount = order[c.name] || 0;

    if (coffeeAmount > c.quantity) {
      alert(`ERROR: You cannot order more ${c.name} than available.`);
      order[c.name] = c.quantity;
      return sum + c.quantity * c.price;
    }

    return sum + coffeeAmount * c.price;
  }, 0);
}

function updatePayment() {
  payment.totalAmount = denominations.reduce(
    (sum, d) => sum + d * cash[d],
    0
  );

  payment.coins = [500, 100, 50, 25]
    .flatMap(d => Array(cash[d]).fill(d));

  payment.bills = Array(cash[1000]).fill(1000);
}

const canBuy = computed(() =>
  total.value > 0 && payment.totalAmount >= total.value
);

async function buyCoffee() {
  try {
    const cleanOrder = Object.fromEntries(
      Object.entries(order).filter(([, qty]) => qty > 0)
    );

    const body = {
      order: cleanOrder,
      payment: {
        totalAmount: payment.totalAmount,
        coins: payment.coins,
        bills: payment.bills
      }
    };

    const resp = await api.post("/buyCoffee", body);

    // Success:
    alert("Compra realizada con éxito\n" + resp.data);

    result.value = resp.data;
    errorMessage.value = "";

    window.location.reload();

  } catch (e) {
    result.value = null;

    errorMessage.value = e.response?.data ?? "Fallo al realizar la compra";

    alert(errorMessage.value);
  }
}

</script>


<style scoped>
.container {
  max-width: 700px;
  margin: auto;
  padding: 20px;
}

.coffee-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.money-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 10px;
}

.buy-btn {
  width: 100%;
  padding: 12px;
  font-size: 18px;
  margin-top: 10px;
}

.result-box {
  background: #eef;
  padding: 15px;
  margin-top: 15px;
  border-radius: 8px;
}

.error-msg {
  margin-top: 10px;
  color: red;
  font-weight: bold;
}

</style>
