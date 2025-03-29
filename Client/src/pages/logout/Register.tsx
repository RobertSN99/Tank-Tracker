import "./Register.css";
const Register = () => {
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const email = formData.get("email") as string;
    const password = formData.get("password") as string;
    const username = formData.get("name") as string;
    console.log({ username, email, password });
  };
  return (
    <div className="container">
      <h1>Register</h1>
      <form className="register-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            type="text"
            id="name"
            name="name"
            placeholder="Username"
            required
            autoFocus={true}
          />
        </div>
        <div className="form-group">
          <input
            type="email"
            id="email"
            name="email"
            placeholder="E-mail"
            required
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
          <input
            type="password"
            id="confirm-password"
            name="confirm-password"
            placeholder="Confirm Password"
            required
          />
        </div>
        <div className="form-group">
          <a href="#" className="login-link">
            Already have an account? Login
          </a>
        </div>
        <button type="submit">Sign Up</button>
      </form>
    </div>
  );
};

export default Register;
