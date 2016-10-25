package application;

import java.math.BigDecimal;

import application.Calculator.CalculatorOperator;

public abstract class BaseCalculatorState implements Calculator{
	protected BigDecimal accumulator;
	protected BigDecimal screenValue;
	protected CalculatorOperator currentCalculatorOperator;
	
	protected final static int MAX_NUMBER_LENGTH = 10;
	
	protected BaseCalculatorState(BigDecimal accumulator, BigDecimal screenValue, CalculatorOperator currentCalculatorOperator) {
		super();
		this.accumulator = accumulator;
		this.screenValue = screenValue;
		this.currentCalculatorOperator = currentCalculatorOperator;
	}

	protected void reset() {
		accumulator = BigDecimal.ZERO;
		screenValue = BigDecimal.ZERO;
		currentCalculatorOperator = CalculatorOperator.ADD;
	}
	
	protected int getNumberOfDecimalPlaces(BigDecimal bigDecimal) {
		return Math.max(0, bigDecimal.scale());
	}
	
	protected boolean thereIsPlaceForMoreDigitsOnScreen(){
		return screenValue.toString().length() < MAX_NUMBER_LENGTH;
	}
	
	protected boolean accumulatorIsTooBig(){
		return accumulator.precision() > MAX_NUMBER_LENGTH;
	}
	
	protected String bigDecimalToShortString(BigDecimal input){
		System.out.println("BigDecim is "+input+" prec is "+input.precision()+" scale is "+input.scale());
		if( input.precision()  > MAX_NUMBER_LENGTH ){
			return input.toString().substring(0, MAX_NUMBER_LENGTH);
		} else{
			return input.toString();
		}
	}

}
