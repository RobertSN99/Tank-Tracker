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
    <div className="container">
      <h1>Sign In</h1>
      <form className="login-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            type="email"
            id="email"
            name="email"
            placeholder="E-mail"
            required
            autoFocus={true}
          />
        </div>
        <div className="form-group">
          <input
            type="password"
            id="password"
            name="password"
            placeholder="Password"
            required
          />
        </div>
        <div className="form-group forgot-password-group">
          <a href="#" className="forgot-password">
            I forgot my password
          </a>
          <a href="#" className="not-a-member">
            Sign up
          </a>
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default Login;
