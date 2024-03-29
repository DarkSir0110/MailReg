﻿using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MailRegWpf
{
	internal static class HttpHelper
	{
		private static readonly String _serverHostUrl;

		static HttpHelper()
		{
			// todo: use config manager
			_serverHostUrl = "http://localhost:5000";
		}

		public static async Task<TResult> GetAsync<TResult>(String apiUrl)
		{
			using HttpClient client = CreateHttpClient();
			using HttpResponseMessage response = await client.GetAsync(_serverHostUrl + apiUrl);

			if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

			String result = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<TResult>(result);
		}

		public static async Task<Guid> PostAsync<TData>(TData data, String apiUrl)
		{
			using HttpClient client = CreateHttpClient();
			using HttpContent content = CreateHttpContent(data);
			using HttpResponseMessage response = await client.PostAsync(_serverHostUrl + apiUrl, content);

			if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

			String result = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<Guid>(result);
		}

		public static async Task PutAsync<TData>(TData data, String apiUrl)
		{
			using HttpClient client = CreateHttpClient();
			using HttpContent content = CreateHttpContent(data);
			using HttpResponseMessage response = await client.PutAsync(_serverHostUrl + apiUrl, content);

			if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
		}

		public static async Task DeleteAsync(String apiUrl)
		{
			using HttpClient client = CreateHttpClient();
			using HttpResponseMessage response = await client.DeleteAsync(_serverHostUrl + apiUrl);

			if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
		}

		private static HttpClient CreateHttpClient()
		{
			var httpClientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
			};

			var client = new HttpClient(httpClientHandler);
			
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return client;
		}

		private static HttpContent CreateHttpContent<TData>(TData data)
		{
			String serialized = JsonConvert.SerializeObject(data);
			return new StringContent(serialized, Encoding.Unicode, "application/json");
		}
	}
}