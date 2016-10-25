package application;

import java.math.BigDecimal;
import java.util.Optional;

public class ScreenViewCalculatorState extends BaseCalculatorState {


	protected ScreenViewCalculatorState(BigDecimal accumulator, BigDecimal screenValue,
			CalculatorOperator initialCalculatorOperator) {
		super(accumulator, screenValue, initialCalculatorOperator);
	}

	@Override
	public Calculator numericButtonPressed(int button) {
		if( thereIsPlaceForMoreDigitsOnScreen()){
		int numberOfDecimalPlaces = getNumberOfDecimalPlaces(screenValue);
			if (numberOfDecimalPlaces == 0) {
				screenValue = screenValue.multiply(new BigDecimal(10)).add(new BigDecimal(button));
			} else {
				BigDecimal thingToAdd = new BigDecimal("0.1").pow(numberOfDecimalPlaces + 1)
						.multiply(new BigDecimal(button));
				screenValue = screenValue.add(thingToAdd);
			}
			return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator);
		} else {
			return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator);
		}
	}
	
	private Optional<Calculator> executeOperation(CalculatorOperator operator){
		if( operator.equals(CalculatorOperator.DIVIDE) &&( screenValue.compareTo(BigDecimal.ZERO) == 0)){
			return Optional.of(new ErrorCalculatorState("DIV/0"));
		}
		accumulator = currentCalculatorOperator.executeAction(accumulator, screenValue);
		if( accumulatorIsTooBig() ){
			return Optional.of(new ErrorCalculatorState(bigDecimalToShortString(accumulator)+"E"));
		}
		return Optional.empty();
	}

	@Override
	public Calculator operatorButtonPressed(CalculatorOperator newOperator) {
		return executeOperation(newOperator)
				.orElse(new ScreenViewCalculatorState(accumulator, BigDecimal.ZERO, newOperator));	
	}

	@Override
	public Calculator dotButtonPressed() {
		return new DotPressedCalculatorState(accumulator, screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator clearButtonPressed() {
		return new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD);
	}

	@Override
	public Calculator invertSignButtonPressed() {
		return new ScreenViewCalculatorState(accumulator, screenValue.negate(), currentCalculatorOperator);
	}

	@Override
	public Calculator squareRootButtonPressed() {
		return new ScreenViewCalculatorState(new BigDecimal(Math.sqrt(screenValue.doubleValue())), screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator equalButtonPressed() {
		return executeOperation(currentCalculatorOperator).orElse(
				new AccumulatorViewCalculatorState(accumulator, screenValue, currentCalculatorOperator));
	}

	@Override
	public String getDisplayedValue() {
		return bigDecimalToShortString(screenValue);
	}

}
