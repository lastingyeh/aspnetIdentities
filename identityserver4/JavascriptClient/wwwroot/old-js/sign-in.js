var signIn = () => {
	var redirectUri = 'https://localhost:5004/Home/SignIn';
    var clientId = 'client_id_js';
	var responseType = 'id_token token';
	var scope = 'openid ApiOne.user';

	var authUrl = `/connect/authorize/callback?client_id=${clientId}&response_type=${encodeURIComponent(responseType)}&redirect_uri=${encodeURIComponent(redirectUri)}&scope=${encodeURIComponent(scope)}&state=${createState()}&nonce=${createNonce()}`;

	var returnUrl = encodeURIComponent(authUrl);

	window.location.href = `https://localhost:5000/Auth/Login?ReturnUrl=${returnUrl}`;
};

var createNonce = () => 'NonceValuerandomValueqwertyuasdffghzxcvbn';
var createState = () => 'StateValuernadomValuepoiuytrelkjhggbnvccf';
