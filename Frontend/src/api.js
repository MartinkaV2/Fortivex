// src/api.js
import axios from "axios";

export default axios.create({
  baseURL: "http://localhost:5001", // ide jön a backend címe
});
