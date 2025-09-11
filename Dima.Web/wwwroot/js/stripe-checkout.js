window.checkout = (stripePublicKey, sessionId) => {
    let stripe = Stripe(sessionId);
    stripe.redirectToCheckout({ 
        sessionId: sessionId 
    });
}