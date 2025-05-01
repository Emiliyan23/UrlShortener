<template>
    <div class="url-shortener">
      <h2>Shorten Your URL</h2>
      <input v-model="url" placeholder="Enter your URL" />
      <button @click="shortenUrl">Shorten URL</button>
  
      <div v-if="shortenedUrl" class="result">
        Shortened URL: 
        <a :href="shortenedUrl" target="_blank">{{ shortenedUrl }}</a>
      </div>
  
      <div v-if="error" class="error">
        {{ error }}
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

  export default {
    name: 'UrlShortener',
    data() {
      return {
        url: '',
        shortenedUrl: '',
        error: ''
      };
    },
    methods: {
      async shortenUrl() {
        this.error = '';
        this.shortenedUrl = '';

        if (!API_BASE_URL) {
         this.error = 'API Base URL is not configured. Please check environment variables.';
         console.error("Error: VITE_API_BASE_URL (or VUE_APP_API_BASE_URL) is not defined in your .env files.");
         return;
        }
        const apiUrl = `${API_BASE_URL}/shorten`;
  
        try {
          // Adjust the base URL if necessary.
          const response = await axios.post(apiUrl, { url: this.url });
          this.shortenedUrl = response.data.shortenedUrl;
        } catch (err) {
          this.error = err.response?.data || 'An error occurred while shortening the URL.';
        }
      }
    }
  };
  </script>
  
  <style scoped>
  .url-shortener {
    max-width: 600px;
    margin: 0 auto;
    padding: 1rem;
  }
  input {
    padding: 0.5rem;
    width: 100%;
    margin-bottom: 0.5rem;
  }
  button {
    padding: 0.5rem 1rem;
    margin-bottom: 1rem;
  }
  .result a {
    color: #007acc;
    text-decoration: none;
  }
  .error {
    color: red;
  }
  </style>
  