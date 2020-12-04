@extends('frontend.layout')

@section('content')
    <header class="masthead text-white text-center" style="background-image: url('{{ Custom::getBackground() }}');">
        <div class="container">
            <div class="row full-height align-items-center">
                <div class="col-12">
                    <div class="row">
                        <div class="col-md-10 col-lg-8 col-xl-7 mx-auto">
                            <div class="card text-body">
                                <div class="card-body">
                                    <div class="row align-items-center">
                                        @if(!empty($error))
                                            <div class="col-12 text-center pb-3">
                                                <div class="alert alert-danger" role="alert">{{ $error }}</div>
                                            </div>
                                        @endif
                                        <div class="col-4 text-center">
                                            <img alt="{{ config('app.name', 'Laravel') }} - @lang('Password')" width="100%" src="{{ asset('img/graphics/authentication.svg') }}">
                                        </div>
                                        <div class="col-8">
                                            <p class="text-left">@lang('The link you are trying to access is password protected').</p>
                                            <form action="{{ $link->getLink() }}" method="POST">
                                                @csrf
                                                <input name="password" type="password" class="form-control" placeholder="@lang('Password')" required>
                                                <button type="submit" class="mt-3 btn btn-primary float-right">@lang('Enter')</button>
                                            </form>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
@endsection
