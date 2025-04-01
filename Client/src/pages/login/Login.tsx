import "./Login.css";
function Login() {
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const email = formData.get("email") as string;
    const password = formData.get("password") as string;
    console.log({ email, password });
  };
  return (
    <div className="login-container">
      <h1 className="login-container-title">Sign In</h1>
      <form className="login-form" onSubmit={handleSubmit}>
        <div className="login-form-group">
          <input
            className="login-input"
            type="email"
            id="email"
            name="email"
            placeholder="E-mail"
            required
            autoFocus={true}
          />
        </div>
        <div className="separator" />
        <div className="login-form-group">
          <input
            className="login-input"
            type="password"
            id="password"
            name="password"
            placeholder="Password"
            required
          />
        </div>
        <div className="login-form-group forgot-password-group">
          <a href="#" id="forgot-password-link">
            I forgot my password
          </a>
          <a href="/register" id="register-link">
            Sign up
          </a>
        </div>
        <button type="submit" id="login-submit-btn">
          Login
        </button>
      </form>
    </div>
  );
}

export default Login;
