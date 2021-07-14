// var config = {
// 	userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
// 	authority: 'https://localhost:5000',
// 	client_id: 'client_id_js',
// 	response_type: 'id_token token',
// 	redirect_uri: 'https://localhost:5004/home/signin',
// 	post_logout_redirect_uri: 'https://localhost:5004/home/index',
// 	scope: 'openid ApiOne.user ApiTwo.sec rc.scope',
// };

// use in Pkce
var config = {
	userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
	authority: 'https://localhost:5000',
	client_id: 'client_id_js',
	response_type: 'code',
	redirect_uri: 'https://localhost:5004/home/signin',
	post_logout_redirect_uri: 'https://localhost:5004/home/index',
	scope: 'openid ApiOne.user ApiTwo.sec rc.scope',
};

var userManager = new Oidc.UserManager(config);

var signIn = () => {
	userManager.signinRedirect();
};

var signOut = () => {
	userManager.signoutRedirect();
};

userManager.getUser().then(user => {
	console.log('user', user);

	if (user) {
		axios.defaults.headers.common['Authorization'] = `Bearer ${user.access_token}`;
	}
});

var callApi = () => {
	axios.get('https://localhost:5001/secret').then(res => {
		console.log(res);
	});
};

var refreshing = false;

axios.interceptors.response.use(
	res => {
		return res;
	},
	err => {
		console.log('axios error:', err.response);

		var axiosConfig = err.response.config;

		// 401 try to refresh token
		if (err.response.status === 401) {
			console.log('axios error 401');

			if (!refreshing) {
				console.log('starting refresh token');

				refreshing = true;

				return userManager.signinSilent().then(user => {
					console.log('new user:', user);
					// update client
					axios.defaults.headers.common['Authorization'] = `Bearer ${user.access_token}`;
					// update http request
					axiosConfig.headers['Authorization'] = `Bearer ${user.access_token}`;

					return axios(axiosConfig);
				});
			}
		}

		return Promise.reject(error);
	}
);
